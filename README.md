# 🚚 MoveSmart Logistics

**Sistema Integral de Gestión Logística y Transporte para Nicaragua.**

MoveSmart es una aplicación de escritorio desarrollada en **C# (Windows Forms)** diseñada para optimizar las operaciones de una empresa de transporte. El sistema implementa estructuras de datos avanzadas para resolver problemas reales de logística, gestión de personal y atención al usuario mediante Inteligencia Artificial.

---

## 📥 Instrucciones para Evaluación (Profesor / Jurado)

Este repositorio contiene el código fuente limpio (sin credenciales). **Para evaluar el proyecto funcionalmente con la API Key ya configurada:**

1.  **Descargar:** Localiza el archivo **`movesmart.rar`** en la lista de archivos de este repositorio y descárgalo.
2.  **Descomprimir:** Extrae el contenido del archivo `.rar`.
3.  **Abrir:** Entra en la carpeta extraída y abre el archivo de solución (`.sln`) con **Visual Studio**.
4.  **Ejecutar:** Compila e inicia el proyecto.

> **Nota:** El archivo `.rar` incluye el fichero `ApiKey.cs` con las credenciales necesarias para que el Chatbot funcione inmediatamente.

---

## 📋 Características Principales

### 🗺️ 1. Optimización de Rutas (Módulo Grafos)
- **Cálculo de Ruta Óptima:** Utiliza el **Algoritmo de Dijkstra** para encontrar el camino más corto entre ciudades de Nicaragua.
- **Visualización Geográfica:** Mapa interactivo calibrado con coordenadas reales sobre el mapa del INETER.
- **Simulación GPS:** Animación en tiempo real de un vehículo recorriendo la ruta calculada.
- **Rutas Mixtas:** Distinción visual y lógica entre carreteras terrestres (Gris) y rutas acuáticas/pangas (Azul).

### 🌳 2. Gestión de Personal (Módulo Árboles)
- **Jerarquía Recursiva:** Implementación de un **Árbol N-ario** para representar la estructura organizacional (CEO -> Gerentes -> Subordinados).
- **Gestión Visual:** Visualización clara mediante `TreeView`.
- **Persistencia:** Guardado y cargado automático de datos.

### 🤖 3. Asistente Virtual IA
- **Integración con Google Gemini:** Chatbot inteligente capaz de responder preguntas sobre la empresa y logística.
- **Comunicación Asíncrona:** Interfaz fluida que no congela la aplicación mientras procesa respuestas.

---

## 🛠️ Tecnologías y Arquitectura

* **Lenguaje:** C# (.NET Framework)
* **Interfaz:** Windows Forms (WinForms)
* **IA:** Google Gemini API
* **Persistencia:** BinaryFormatter

### Estructura del Proyecto
```text
MoveSmart_App
│
├── 📂 Modelos          (Entidades)
├── 📂 Estructuras      (Lógica: Grafos, Árboles, API)
├── 📂 Vistas           (Interfaz de Usuario)
└── 📄 ApiKey.cs        (Solo incluido en el .rar por seguridad)
