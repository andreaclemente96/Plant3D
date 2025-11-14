import open3d as o3d
import numpy as np
import os
import sys

def create_mesh_from_point_cloud(pcd_segment, class_label):
    if len(pcd_segment.points) < 50:
        print(f"No hay suficientes puntos para la clase {class_label}.")
        return None

    print(f"🛠️ Creando malla para la clase {class_label} con {len(pcd_segment.points)} puntos...")

    # Estimar las normales 
    pcd_segment.estimate_normals(search_param=o3d.geometry.KDTreeSearchParamHybrid(radius=0.1, max_nn=30))

    # Usar un algoritmo diferente para cada clase
    if class_label == 0:  # 0 = tallo
        # Reconstrucción de Balas para el tallo (denso)
        radii = [0.01, 0.02, 0.04, 0.08, 0.15]
        mesh = o3d.geometry.TriangleMesh.create_from_point_cloud_ball_pivoting(
            pcd_segment, o3d.utility.DoubleVector(radii)
        )
    else:
        # Reconstrucción de la superficie de las hojas de forma manual
        # Se crea un plano 2D y se ajusta a la forma de los puntos
        points_2d = np.asarray(pcd_segment.points)[:, :2]
        
        # Se crea una malla triangulando los puntos en el plano 2D
        triangles = o3d.geometry.TriangleMesh.create_from_point_cloud_alpha_shape(pcd_segment, alpha=0.5)
        
        # Se suaviza la malla
        triangles = triangles.filter_smooth_simple(number_of_iterations=5)
        
        mesh = triangles

    # Suavizar la malla resultante
    mesh.compute_vertex_normals()
    
    return mesh

def main_reconstruction(input_ply_path):
    if not os.path.exists(input_ply_path):
        print(f"Error: No se encontró el archivo en la ruta: {input_ply_path}")
        return

    print(f"Cargando la nube de puntos segmentada desde: {input_ply_path}")
    pcd = o3d.io.read_point_cloud(input_ply_path)
    
    if not pcd.has_points():
        print("El archivo PLY no contiene puntos válidos.")
        return

    points = np.asarray(pcd.points)
    colors = np.asarray(pcd.colors)

    unique_colors = np.unique(colors, axis=0)

    mesh_list = []
    for i, color_label in enumerate(unique_colors):
        indices = np.where(np.all(colors == color_label, axis=1))[0]
        
        pcd_segment = o3d.geometry.PointCloud()
        pcd_segment.points = o3d.utility.Vector3dVector(points[indices])
        pcd_segment.colors = o3d.utility.Vector3dVector(colors[indices])
        
        mesh = create_mesh_from_point_cloud(pcd_segment, i)
        
        if mesh:
            mesh_list.append(mesh)
            output_mesh_path = f"malla_segmento_{i}.obj"
            o3d.io.write_triangle_mesh(output_mesh_path, mesh)
            print(f"Malla guardada en: {output_mesh_path}")
            
    if mesh_list:
        print("\nVisualizando todas las mallas. Cerrar al terminar.")
        o3d.visualization.draw_geometries(mesh_list)
    else:
        print("No se crearon mallas para visualizar.")

if __name__ == "__main__":
    input_file = r"Rutadeseada\planta_segmentada.ply"

    main_reconstruction(input_file)
