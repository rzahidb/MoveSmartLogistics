# 🚚 MoveSmart Logistics

**Sistema Integral de Gestión Logística y Transporte para Nicaragua.**

MoveSmart es una aplicación de escritorio desarrollada en **C# (Windows Forms)** diseñada para optimizar las operaciones de una empresa de transporte. El sistema implementa estructuras de datos avanzadas para resolver problemas reales de logística, gestión de personal y atención al usuario mediante Inteligencia Artificial.

---

## 📋 Características Principales

### 🗺️ 1. Optimización de Rutas (Módulo Grafos)
- **Cálculo de Ruta Óptima:** Utiliza el **Algoritmo de Dijkstra** para encontrar el camino más corto entre ciudades de Nicaragua.
- **Visualización Geográfica:** Mapa interactivo calibrado con coordenadas reales sobre el mapa del INETER.
- **Simulación GPS:** Animación en tiempo real de un vehículo recorriendo la ruta calculada.
- **Rutas Mixtas:** Distinción visual y lógica entre carreteras terrestres (Gris) y rutas acuáticas/pangas (Azul), cubriendo Pacífico, Centro, Norte y Regiones Autónomas (RAAN/RAAS).

### 🌳 2. Gestión de Personal (Módulo Árboles)
- **Jerarquía Recursiva:** Implementación de un **Árbol N-ario** para representar la estructura organizacional (CEO -> Gerentes -> Subordinados).
- **Gestión Visual:** Visualización clara mediante `TreeView`.
- **Operaciones:** Agregar nuevos empleados bajo cualquier jefatura existente.
- **Persistencia:** Guardado y cargado automático de datos mediante serialización binaria (`.bin`).

### 🤖 3. Asistente Virtual IA
- **Integración con Google Gemini:** Chatbot inteligente capaz de responder preguntas sobre la empresa y logística en lenguaje natural.
- **Comunicación Asíncrona:** Interfaz fluida que no se congela mientras espera la respuesta de la API.

### 🔒 4. Seguridad y Reportes
- **Login Seguro:** Control de acceso con límite de 3 intentos fallidos.
- **Reportes Detallados:** Generación de itinerarios paso a paso, listados de personal y análisis de cobertura vial.

---

## 🛠️ Tecnologías y Arquitectura

El proyecto sigue una **Arquitectura en Capas (N-Tier)** para garantizar la modularidad y el mantenimiento:

* **Lenguaje:** C# (.NET Framework / .NET 6+)
* **Interfaz:** Windows Forms (WinForms)
* **Gráficos:** GDI+ (System.Drawing)
* **IA:** Google Gemini API (REST/JSON)
* **Persistencia:** BinaryFormatter

### Estructura del Proyecto
```text
MoveSmart_App
│
├── 📂 Modelos       (Entidades: Ciudad, Ruta, Empleado)
├── 📂 Estructuras   (Lógica: GrafoRutas, ArbolEmpresa, ChatbotLogic)
├── 📂 Vistas        (UI: MapaUI, VistaArbol, MenuPrincipal, Login)
└── Program.cs       (Punto de entrada)
