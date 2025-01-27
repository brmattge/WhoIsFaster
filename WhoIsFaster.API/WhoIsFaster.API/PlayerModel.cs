namespace WhoIsFaster.API
{
    public class PlayerModel
    {
        public string Name { get; set; }
        public string ConnectionId { get; set; }
        public double TotalTime { get; set; } // Tempo total acumulado
        public DateTime LastClickTime { get; set; } = DateTime.UtcNow; // Último clique
    }

}
