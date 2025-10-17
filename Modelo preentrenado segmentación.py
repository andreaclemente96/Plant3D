import torch
import torch.nn as nn
import open3d as o3d
import numpy as np
import pandas as pd
import os
import sys
from typing import Dict, List

# repo
REPO_ROOT = r"rutarepositorio\PlantNet-and-PSegNet"
PSegNet_PATH = os.path.join(REPO_ROOT, "PSegNet", "PSegNet_pytorch")
sys.path.append(PSegNet_PATH)
sys.path.append(os.path.join(PSegNet_PATH, "models"))
sys.path.append(os.path.join(PSegNet_PATH, "utils"))

# Importar la clase del modelo
from models.model_pytorch import plantnet_model

# Configuración del modelo y las clases
MODEL_PATH = os.path.join(PSegNet_PATH, "models", "checkpoints", "model_epoch199.pth")
NUM_CLASSES = 6
CLASSES = ["tallo", "hoja", "desconocido_2", "desconocido_3", "desconocido_4", "desconocido_5"]
CLASS_COLORS = {
 0: [139, 69, 19],
 1: [34, 139, 34],
 2: [128, 128, 128],
 3: [255, 0, 0],
 4: [0, 0, 255],
 5: [255, 255, 0]
}

# Procesar y visualizar
def load_txt_and_predict(txt_path: str, model: torch.nn.Module, class_map: Dict, output_path: str, chunk_size=4096):
    if not os.path.exists(txt_path):
        raise FileNotFoundError(f"El archivo no se encontró en la ruta: {txt_path}")

    total_points = 0
    all_points_processed = o3d.geometry.PointCloud()

    model.eval()
    with torch.no_grad():
        for chunk in pd.read_csv(txt_path, delim_whitespace=True, header=None, usecols=[0,1,2], chunksize=chunk_size):
            points = chunk.values.astype(np.float32)

            if points.shape[0] < 2:
                continue

            points = points.reshape(-1, 3)
            total_points += points.shape[0]

            points_tensor = torch.from_numpy(points).unsqueeze(0)
            outputs, _, _ = model(points_tensor)
            predicted_labels = torch.argmax(outputs.squeeze(), dim=1).cpu().numpy()

            colors = np.zeros_like(points)
            for i, color_rgb in enumerate(list(class_map.values())):
                indices = predicted_labels == i
                colors[indices] = color_rgb
            
            pcd_chunk = o3d.geometry.PointCloud()
            pcd_chunk.points = o3d.utility.Vector3dVector(points)
            pcd_chunk.colors = o3d.utility.Vector3dVector(colors / 255.0)
            
            all_points_processed += pcd_chunk

    o3d.io.write_point_cloud(output_path, all_points_processed)
    return all_points_processed, np.asarray(all_points_processed.points)

def save_and_visualize(pcd: o3d.geometry.PointCloud, labels: np.ndarray, output_path: str, classes: List[str]):
    o3d.io.write_point_cloud(output_path, pcd)

    unique_labels, counts = np.unique(labels, return_counts=True)
    for label, count in zip(unique_labels, counts):
        label_int = int(label)
        if label_int < len(classes):
            print(f"- {classes[label_int]}: {count} puntos")
        else:
            print(f"- Clase desconocida ({label_int}): {count} puntos")

    o3d.visualization.draw_geometries([pcd])

# Ejecución principal
if __name__ == "__main__":
    input_file = r"rutaarchivo.txt"
    output_file = "planta_segmentada.ply"

    try:
        model = plantnet_model(num_classes=NUM_CLASSES)
        checkpoint = torch.load(MODEL_PATH, map_location=torch.device('cpu'))
        model.load_state_dict(checkpoint['model_state_dict'])
        
        segmented_pcd, labels = load_txt_and_predict(input_file, model, CLASS_COLORS, output_path=output_file)
        
        save_and_visualize(segmented_pcd, labels, output_file, CLASSES)
        
    except FileNotFoundError as e:
        print(f"Error: {e}. Comprobar repo.")
    except Exception as e:
        print(f"error inesperado: {e}")