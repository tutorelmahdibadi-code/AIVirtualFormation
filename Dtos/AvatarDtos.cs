namespace VIRTUAL_CLASSE_FORMATION.Dtos
{
    // Demande de demarrage d'une session avatar pour presenter un module
    public class StartAvatarSessionDto
    {
        public int FormateurId { get; set; }
        public int ModuleFormationId { get; set; }
    }

    // Le frontend utilise ce token avec le SDK JS Anam pour demarrer
    // le flux video/audio directement depuis le navigateur (pas besoin
    // de relayer les questions/reponses par notre backend : la conversation
    // se fait directement entre le navigateur et Anam via leur SDK).
    public class AvatarSessionDto
    {
        public string SessionToken { get; set; } = string.Empty;
    }
}
