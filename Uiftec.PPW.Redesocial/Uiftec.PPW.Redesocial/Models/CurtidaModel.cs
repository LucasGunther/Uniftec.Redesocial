namespace Uiftec.PPW.Redesocial.Models
{
    public class CurtidaModel
    {
        public Guid Id { get; set; } // Identificador único da curtida
        public Guid UserId { get; set; } // Identificador do usuário que curtiu
        public Guid PostId { get; set; } // Identificador do post que foi curtido
        public DateTime DataCurtida { get; set; } // Data e hora em que a curtida foi feita
    }
}
