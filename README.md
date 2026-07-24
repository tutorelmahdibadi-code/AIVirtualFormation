# g2soir — Plateforme de gestion des formations

API ASP.NET Core (.NET 8) couvrant le cahier des charges :
authentification par rôle, gestion des catégories/formations/formateurs/modules/modalités,
inscription des apprenants, et intégration d'un formateur avatar IA.

## Installation

1. Ouvrir `VIRTUAL_CLASSE_FORMATION.csproj` dans Visual Studio 2022, ou en ligne de commande :
   ```
   dotnet restore
   ```
2. Configurer `appsettings.json` :
   - `ConnectionStrings:myconn` → ta base MySQL (ou SQL Server, voir plus bas)
   - `Jwt:Key` → remplacer par une vraie clé secrète (32+ caractères)
   - `AnamIA:ApiKey` → ta clé API Anam.ai (voir section dédiée plus bas)

3. Créer les migrations et la base :
   ```
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```

4. Lancer :
   ```
   dotnet run
   ```
   Swagger disponible sur `/swagger` en environnement Development.

## Utiliser SQL Server au lieu de MySQL

Remplacer dans `g2soir.csproj` le package `Pomelo.EntityFrameworkCore.MySql` par
`Microsoft.EntityFrameworkCore.SqlServer`, et dans `Program.cs` :
```csharp
options.UseSqlServer(connectionString)
```
au lieu de `UseMySql(...)`.

## Rôles et authentification

- `POST /api/auth/register` — crée un compte (Apprenant par défaut). Un Admin peut
  créer des comptes Formateur en envoyant `"role": "Formateur"` dans le body.
- `POST /api/auth/login` — retourne un JWT à mettre dans l'en-tête
  `Authorization: Bearer {token}` pour les routes protégées.
- Rôles : `Admin`, `Formateur`, `Apprenant` (voir `Models/Role.cs`).

## Endpoints principaux

| Ressource     | Routes                                                        |
|---------------|----------------------------------------------------------------|
| Catégories    | `GET/POST /api/categories`, `GET/PUT/DELETE /api/categories/{id}` |
| Formateurs    | `GET/POST /api/formateurs`, `GET/PUT/DELETE /api/formateurs/{id}` |
| Formations    | `GET/POST /api/formations`, `GET/PUT/DELETE /api/formations/{id}` |
| Modules       | `GET /api/modules/formation/{formationId}`, `POST/PUT/DELETE /api/modules` |
| Modalités     | `GET /api/modalites/module/{moduleId}`, `POST/PUT/DELETE /api/modalites` |
| Inscriptions  | `POST /api/inscriptions?apprenantId=&formationId=`, `PUT /api/inscriptions/{id}/progression?progression=` |
| Avatar IA     | `POST /api/avatar/session`                                     |

## Formateur IA — intégration Anam.ai

L'intégration utilise l'API réelle d'Anam.ai (https://docs.anam.ai) :

1. Crée un compte sur [Anam Lab](https://lab.anam.ai), génère une **clé API**,
   choisis/crée un avatar (`avatarId`, `avatarModel`), une voix (`voiceId`)
   et un modèle LLM (`llmId`).
2. Mets ces valeurs dans la fiche du `Formateur` correspondant (via
   `POST /api/formateurs`) — c'est ce `Formateur` qui sera "joué" par l'avatar.
3. Mets ta clé API dans `appsettings.json` → `AnamIA:ApiKey`.
4. Le flux :
   - Le frontend appelle `POST /api/avatar/session` (authentifié, body
     `{ formateurId, moduleFormationId }`)
   - Le backend échange la clé API contre un **jeton de session temporaire**
     via `POST https://api.anam.ai/v1/auth/session-token`, en injectant le
     contenu du module dans le prompt système
   - Le backend renvoie ce `sessionToken` au frontend
   - Le frontend utilise le SDK JS `@anam-ai/js-sdk` avec ce token pour
     démarrer directement le flux vidéo/audio en temps réel — **la conversation
     ne passe pas par notre backend**, elle se fait directement entre le
     navigateur et Anam (c'est pour ça qu'il n'y a qu'un seul endpoint,
     pas de "poser une question")

Une page de test est fournie : lance le projet et ouvre `/avatar-test.html`
dans le navigateur (Chrome/Edge, autorise le micro). Connecte-toi avec un
compte existant, indique l'id d'un Formateur et d'un Module, et démarre.

## Frontend

Ce projet est une API pure (pas de vues Razor/Blazor), à l'exception de la
page de test `wwwroot/avatar-test.html`. Pour le reste de l'application
(admin, catalogue de formations, espace apprenant...), il faudra un frontend
séparé (React, Angular, Blazor WASM...) qui consomme ces endpoints.

## Sécurité
- Changer `Jwt:Key` avant toute mise en ligne
- Restreindre la policy CORS (`AllowAnyOrigin`) à l'URL réelle du frontend
- Ne jamais committer `appsettings.json` avec de vraies clés — utiliser
  `dotnet user-secrets` ou les variables d'environnement Railway
