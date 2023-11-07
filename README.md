# MongoImport
## Description
Outil pour importer rapidement des fichier JSON exportés de MongoDB.

Instructions 

1. Créer un fichier `.env` dans le même dossier que l'éxecutable `mongoimport.exe` avec la varriable `CONNECTION_STRING=<connection_string>` si vous désirer une connection string autre que celle par default (`mongodb://localhost:27017`)
2. Dans le dossier `Json` placer tout les exports JSON de MongoDB. Ils devraient avoir le format suivant : `Database.Collection.json`.
3. Lancer l'éxecutable `mongoimport.exe`.

