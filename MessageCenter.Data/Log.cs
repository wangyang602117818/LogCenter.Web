﻿using MongoDB.Bson;
using MongoDB.Driver;
using SSO.Util.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageCenter.Data
{
    public class Log : MongoBase
    {
        public Log() : base("Log_" + DateTime.Now.Year) { }
        public IEnumerable<BsonDocument> GetFromList()
        {
            return MongoCollection.Aggregate()
                .Group<BsonDocument>(new BsonDocument()
                {
                    {"_id","$From" },
                    {"count",new BsonDocument("$sum",1) }
                }).ToEnumerable();
        }
        public IEnumerable<BsonDocument> OpRecordDay(DateTime createTime)
        {
            return MongoCollection.Aggregate()
                 .Match(FilterBuilder.Gte("CreateTime", createTime))
                 .Project(new BsonDocument("date", new BsonDocument("$dateToString", new BsonDocument() {
                    {"format", "%Y-%m-%d" },
                    {"timezone",AppSettings.GetValue("timezone") },
                    {"date", "$CreateTime" }}
                 )))
                 .Group<BsonDocument>(new BsonDocument() {
                    {"_id","$date" },
                    {"count",new BsonDocument("$sum",1) }
                 })
                 .Sort(new BsonDocument("_id", 1)).ToEnumerable();
        }
        public IEnumerable<BsonDocument> GetControllersByFrom(string from)
        {
            return MongoCollection.Aggregate()
                .Match(FilterBuilder.Eq("From", from))
                .Group<BsonDocument>(new BsonDocument()
                {
                    {"_id","$Controller" },
                    {"count",new BsonDocument("$sum",1) }
                }).ToEnumerable();
        }
        public IEnumerable<BsonDocument> GetActionsByController(string from, string controller)
        {
            return MongoCollection.Aggregate()
                .Match(FilterBuilder.Eq("From", from) & FilterBuilder.Eq("Controller", controller))
                .Group<BsonDocument>(new BsonDocument()
                {
                    {"_id","$Action" },
                    {"count",new BsonDocument("$sum",1) }
                }).ToEnumerable();
        }
        public long GetCountByDate(DateTime? startTime, DateTime? endTime)
        {
            List<FilterDefinition<BsonDocument>> list = new List<FilterDefinition<BsonDocument>>();
            if (startTime != null) list.Add(FilterBuilder.Gte("CreateTime", startTime.Value));
            if (endTime != null) list.Add(FilterBuilder.Lte("CreateTime", endTime.Value));
            if (list.Count == 0) return MongoCollection.EstimatedDocumentCount();
            return MongoCollection.CountDocuments(FilterBuilder.And(list));
        }
        public IEnumerable<BsonDocument> GetLastValuableRecord()
        {

            return null;
        }
    }
}