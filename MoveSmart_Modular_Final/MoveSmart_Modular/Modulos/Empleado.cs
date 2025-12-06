using System;
using System.Collections.Generic;

namespace ArbolEmpresaMudanzas.Modulo
{
    // Clase que representa un nodo del árbol jerárquico.
    public class Empleado
    {
        public string Nombre { get; set; }
        public string Cargo { get; set; }
        public string Cedula { get; set; }
        public DateTime FechaIngreso { get; set; }

        // Lista recursiva: Un empleado contiene a sus subordinados
        public List<Empleado> Subordinados { get; set; }

        public Empleado() { Subordinados = new List<Empleado>(); } // Constructor vacío para JSON

        public Empleado(string nombre, string cargo, string cedula, DateTime fecha)
        {
            Nombre = nombre; Cargo = cargo; Cedula = cedula; FechaIngreso = fecha;
            Subordinados = new List<Empleado>();
        }

        public void ActualizarDatos(string nombre, string cargo, string cedula, DateTime fecha)
        {
            this.Nombre = nombre; this.Cargo = cargo; this.Cedula = cedula; this.FechaIngreso = fecha;
        }

        public override string ToString() { return $"{Cargo}: {Nombre}"; }
    }
}