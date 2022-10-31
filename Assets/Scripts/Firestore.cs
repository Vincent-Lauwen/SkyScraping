using Firebase.Firestore;
using Firebase.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System;
using Firebase.Auth;
using System.IO;

namespace Vincent.Wanderlost.Code
{
    public class Firestore
    {
        private static readonly FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
       

        //General methods
        public static async Task<T> GetDocumentAsync<T>(string pathToDocument) where T : class
        {
            try
            {
                DocumentReference doc = db.Document(pathToDocument);
                DocumentSnapshot snapshot = await doc.GetSnapshotAsync();
                if (snapshot.Exists)
                {
                    return snapshot.ConvertTo<T>();
                }
                else
                {
                    throw new Exception("Failed to read or find data from server");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static async Task<T> DocumentExistsAsync<T>(string pathToDocument) where T : class
        {
            try
            {
                DocumentReference doc = db.Document(pathToDocument);
                DocumentSnapshot snapshot = await doc.GetSnapshotAsync();
                if (snapshot.Exists)
                {
                    return snapshot.ConvertTo<T>();
                }
                else
                {
                    return default;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static async Task<IDictionary<string, object>> GetDocumentValuesAsync(string pathToDocument, params string[] fieldNames)
        {
            try
            {
                DocumentReference doc = db.Document(pathToDocument);
                DocumentSnapshot snapshot = await doc.GetSnapshotAsync();
                if (snapshot.Exists)
                {
                    IDictionary<string, object> dataDictionary = new Dictionary<string, object>();

                    foreach (string fieldName in fieldNames)
                    {
                        if (snapshot.TryGetValue(fieldName, out object value))
                        {
                            dataDictionary.Add(fieldName, value);
                        }
                    }

                    return dataDictionary;
                }
                else
                {
                    throw new Exception("Failed to read or find data from server");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static async Task CreateDocumentAsync(object data, string pathToCollection, string documentName)
        {
            try
            {
                DocumentReference doc = db.Collection(pathToCollection).Document(documentName);
                await doc.SetAsync(data).ContinueWith(task =>
                {
                    if (!task.IsCompleted)
                    {
                        throw new Exception("Failed to create data to server");
                    }
                });
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static async Task MergeDataInDocumentAsync(object data, string pathToDocument, params string[] fieldNames)
        {
            try
            {
                DocumentReference doc = db.Document(pathToDocument);
                //Will create the document if it not exists
                await doc.SetAsync(data, SetOptions.MergeFields(fieldNames)).ContinueWith(task =>
                {
                    if (!task.IsCompleted)
                    {
                        throw new Exception("Failed to create data to server");
                    }
                });
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static async Task UpdateDocumentAsync(IDictionary<string, object> update, string pathToDocument)
        {
            try
            {
                DocumentReference doc = db.Document(pathToDocument);
                await doc.UpdateAsync(update).ContinueWith(task =>
                {
                    if (!task.IsCompleted)
                    {
                        throw new Exception("Failed to update data to server");
                    }
                });
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static async Task DeleteDocumentAsync(string pathToCollection, string documentName)
        {
            try
            {
                DocumentReference doc = db.Collection(pathToCollection).Document(documentName);
                await doc.DeleteAsync().ContinueWith(task =>
                {
                    if (!task.IsCompleted)
                    {
                        throw new Exception("Failed to delete documentation");
                    }
                });
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static async Task<IList<T>> GetCollectionAsync<T>(string pathToCollection)
        {
            try
            {
                IList<T> dataList = new List<T>();
                CollectionReference col = db.Collection(pathToCollection);
                QuerySnapshot snapshot = await col.GetSnapshotAsync();
                foreach (DocumentSnapshot document in snapshot)
                {
                    T data = document.ConvertTo<T>();
                    dataList.Add(data);
                }
                return dataList;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static async Task<IList<IDictionary<string, object>>> GetCollectionValuesAsync(string pathToCollection, params string[] fieldNames)
        {
            try
            {
                CollectionReference col = db.Collection(pathToCollection);
                QuerySnapshot snapshot = await col.GetSnapshotAsync();

                IList<IDictionary<string, object>> dataList = new List<IDictionary<string, object>>();
                foreach (DocumentSnapshot document in snapshot)
                {
                    if (document.Exists)
                    {
                        IDictionary<string, object> dataDictionary = new Dictionary<string, object>();
                        foreach (string fieldName in fieldNames)
                        {
                            if (document.TryGetValue(fieldName, out object value))
                            {
                                dataDictionary.Add(fieldName, value);
                            }
                        }
                        dataList.Add(dataDictionary);
                    }
                }
                return dataList;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static async Task ClearCollectionAsync(string pathToCollection)
        {
            try
            {
                WriteBatch batch = db.StartBatch();
                CollectionReference col = db.Collection(pathToCollection);

                QuerySnapshot snapshot = await col.GetSnapshotAsync();
                foreach (DocumentSnapshot document in snapshot)
                {
                    batch.Delete(document.Reference);
                }
                await batch.CommitAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }


        //Player methods
        public static async Task SaveUser(object playerData, string pathToDocument, string userId)
        {
            try
            {
                await CreateDocumentAsync(playerData, pathToDocument, userId);
            }
            catch (Exception ex)
            {
                throw new Exception("Could not save player", ex);
            }
        }
        public static async Task<T> GetUser<T>(string pathToDocument) where T : class
        {
            try
            {
                return await GetDocumentAsync<T>(pathToDocument);
            }
            catch (Exception)
            {
                return default;
            }
        }
        public static async Task<T> UserExists<T>(string pathToDocument) where T : class
        {
            try
            {
                return await DocumentExistsAsync<T>(pathToDocument);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while finding if the player's document exists", ex);
            }
        }


        //Achievement methods
        public static async Task<List<Achievement>> GetUserAchievementsAsync(string templateAchievementCollectionPath, string playerAchievementCollectionPath)
        {
            try
            {
                WriteBatch batch = db.StartBatch();
                List<Achievement> playerAchievements = new List<Achievement>();

                //Incase any new achievements are added in Firebase. These will be given to the player's achievement collection.
                foreach (Achievement templateAchievement in await GetCollectionAsync<Achievement>(templateAchievementCollectionPath))
                {
                    Achievement playerAchievement = await DocumentExistsAsync<Achievement>(playerAchievementCollectionPath + "/" + templateAchievement.Id);
                    if (playerAchievement == null)
                    {
                        DocumentReference doc = db.Document(playerAchievementCollectionPath + "/" + templateAchievement.Id);
                        batch.Set(doc, templateAchievement);
                        playerAchievements.Add(templateAchievement);
                    }
                    else
                    {
                        playerAchievements.Add(playerAchievement);
                    }
                }
                await batch.CommitAsync();
                return playerAchievements;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static async Task UpdateUserAchievementAsync(Achievement achievement, string pathToDocument )
        {
            try
            {
                IDictionary<string, object> achievementUpdate = new Dictionary<string, object>
                {
                    {"Progression", achievement.Progression },
                    {"Completed", achievement.Completed },
                };
                await UpdateDocumentAsync(achievementUpdate, pathToDocument);
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Leaderbord methods
        public static async Task<IList<IDictionary<string,object>>> GetLeaderbordAsync(string pathToCollection, int limit, string orderByDescField, params string[] fieldNames)
        {
            try
            {
                CollectionReference col = db.Collection(pathToCollection);
                QuerySnapshot snapshots = await col.OrderByDescending(orderByDescField).Limit(limit).GetSnapshotAsync();

                IList<IDictionary<string, object>> dataList = new List<IDictionary<string, object>>();
                foreach (DocumentSnapshot document in snapshots)
                {
                    if (document.Exists)
                    {
                        IDictionary<string, object> dataDictionary = new Dictionary<string, object>();
                        foreach (string fieldName in fieldNames)
                        {
                            if (document.TryGetValue(fieldName, out object value))
                            {
                                dataDictionary.Add(fieldName, value);
                            }
                        }
                        dataList.Add(dataDictionary);
                    }
                }
                return dataList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Powerup methods
        public static async Task<List<PowerUp>> GetUserPowerups(string playerPowerupCollectionPath)
        {
            try
            {
                return new List<PowerUp>(await GetCollectionAsync<PowerUp>(playerPowerupCollectionPath));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static async Task<IList<PowerUp>> GetShopItems(string templatePowerupCollectionPath)
        {
            try
            {
                return await GetCollectionAsync<PowerUp>(templatePowerupCollectionPath);
            }
            catch(Exception)
            {
                throw;
            }
            
        }

        public static async Task BuyItem(PowerUp powerUp, string playerPowerupCollectionPath)
        {
            try
            {
                await CreateDocumentAsync(powerUp, playerPowerupCollectionPath, powerUp.Id);
            }
            catch (Exception)
            {
                throw;
            }
        }
    } 
}
