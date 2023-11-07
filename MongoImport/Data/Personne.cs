using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel;

namespace MongoImport.Data;

/// <summary>
///  Représente une personne dans la base de données
/// </summary>
/// <typeparam name="T"> Le type de rôle de la personne </typeparam>
public class Personne<T> : INotifyPropertyChanged
{
	/// <summary>
	///  L'identifiant de la personne
	/// </summary>
	[BsonId]
	public ObjectId? Id { get; set; }

	/// <summary>
	///  Le nom de la personne
	/// </summary>
	[BsonElement("nom")]
	public string Nom { get; set; }

	/// <summary>
	///  Le prénom de la personne
	/// </summary>
	[BsonElement("prenom")]
	public string Prenom { get; set; }

	/// <summary>
	///  L'adresse courriel de la personne
	/// </summary>
	[BsonElement("email")]
	public string Email { get; set; }

	/// <summary>
	///  Le numéro de téléphone de la personne
	/// </summary>
	[BsonElement("telephone")]
	public string Telephone { get; set; }

	/// <summary>
	///  Le rôle de la personne 
	/// </summary>
	[BsonElement("role")]
	public T Role { get; set; }

	public event PropertyChangedEventHandler? PropertyChanged;

	protected virtual void OnPropertyChanged(string? propertyName = null)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}