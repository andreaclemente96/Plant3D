ParteInteractiva.cs: Gestiona la selección de componentes del modelo y activa información contextual en la interfaz.

PartSelector.cs: Permite seleccionar partes individuales del modelo con la mano derecha.

ResetManager.cs: Restablece la selección de partes y reinicia la interfaz para nuevas interacciones.

ExportHierarchy.cs: Exporta la jerarquía de objetos de la escena para documentación y trazabilidad.

FollowLeftHandOnGrab.cs: Permite manipular objetos 3D usando la mano izquierda en tiempo real.

HeadLockedUI.cs: Mantiene la interfaz siempre orientada hacia el usuario.

InteractableToggleCollection.cs: Coordina la activación y desactivación de objetos interactivos en colecciones de botones o toggles.


Modelo preentrenado segmentacion.py: Ejecuta la segmentación semántica de una nube de puntos 3D mediante el modelo PlantNet/PSegNet, asignando etiquetas y colores por clase y generando un archivo .ply segmentado.

conversión ply a malla.py: Reconstruye mallas tridimensionales a partir de las nubes de puntos segmentadas, aplicando distintos métodos de triangulación y suavizado según la clase detectada.
