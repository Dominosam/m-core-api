using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Redis.OM.Modeling;


//namespace Mover.Data.Repositories.Inventory.Models
//{
//    [Document(StorageType = StorageType.Json, IndexName = "InventoryItem" )]
//    public class InventoryItem
//    {
//        [RedisIdField]
//        [Indexed]
//        public string? InventoryItemId { get; set; }
//        [Indexed]
//        public string? SKU { get; set; }
//        [Indexed]
//        public string? Description { get; set; }
//        [Indexed]
//        public int Quantity { get; set; }
//    }
//}


namespace Mover.Data.Repositories.Inventory.Models
{
    public class InventoryItem
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("SKU")]
        public string? SKU { get; set; }

        [BsonElement("Description")]
        public string? Description { get; set; }

        [BsonElement("Quantity")]
        public int Quantity { get; set; }
    }
}
