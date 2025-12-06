namespace MoveSmart_Modular.Modelos
{
    public class Ruta
    {
        public Ciudad Destino { get; set; }
        public int DistanciaKm { get; set; }

        // Propiedad para distinguir si se pinta Gris (Carretera) o Azul (Agua)
        public bool EsAcuatica { get; set; }
    }
}