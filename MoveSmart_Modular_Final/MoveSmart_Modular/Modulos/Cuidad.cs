namespace MoveSmart_Modular.Modelos
{
    public class Ciudad
    {
        public string Nombre { get; set; }
        public string Departamento { get; set; }

        // Coordenadas X, Y para dibujar en el Mapa (PictureBox)
        public float X { get; set; }
        public float Y { get; set; }

        public override string ToString() => Nombre;
    }
}