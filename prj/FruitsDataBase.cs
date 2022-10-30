using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Npgsql;
using Dapper;
using System.Collections.ObjectModel;

namespace fruits
{
    class FruitsDataBase
    {
        string ConnStr = "Server=localhost;Port=5432;Database=fruits;User Id=postgres;Password=masterkey;Pooling=true;" +
            "ClientEncoding=utf-8";
        public bool GetPrice(ref float price, int ProvId, int ProdTypeId)
        {
            using (IDbConnection db = new NpgsqlConnection(ConnStr))
            {
                var SQL = $"SELECT prc_price FROM price WHERE provider_id = {ProvId} AND product_type_id = {ProdTypeId} AND NOW() > prc_begin_date AND NOW() < prc_end_date";
                var r = db.Query<float>(SQL);
                if (r.Count() != 1)
                    return false;
                price = r.First();
                return true;
            }
        }

        public void AddDelivery(List<DeliveryPosition> positions)
        {
            using (IDbConnection db = new NpgsqlConnection(ConnStr))
            {
                var DelDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                var SQL = $"INSERT INTO delivery(del_date) VALUES('{DelDate}') RETURNING del_id";
                var r = db.Query<int>(SQL);
                int DelId = r.First();

                foreach (var pos in positions)
                {
                    SQL = $"INSERT INTO delivery_position(del_id,dep_weight,dep_price,prt_id,prv_id,dep_date)" +
                        $" VALUES({DelId},{pos.Weight},{pos.Price},{pos.ProductTypeId},{pos.ProviderId},'{DelDate}')";
                    db.Execute(SQL);
                }
            }
            
        }

        public void GetProductTypes(ref IEnumerable<ProductType> ProdTypes)
        {
            using (IDbConnection db = new NpgsqlConnection(ConnStr))
            {
                var SQL = "SELECT * FROM product_type";
                ProdTypes = db.Query<ProductType>(SQL);
            }
        }

        public IEnumerable<ProductType> GetProductTypes()
        {
            using (IDbConnection db = new NpgsqlConnection(ConnStr))
            {
                var SQL = "SELECT * FROM product_type";
                var ProdTypes = db.Query<ProductType>(SQL);
                return ProdTypes;
            }
        }

        public void GetProviders(ref IEnumerable<Provider> providers)
        {
            using (IDbConnection db = new NpgsqlConnection(ConnStr))
            {
                var SQL = "SELECT * FROM provider";
                providers = db.Query<Provider>(SQL);
            }
        }

        public IEnumerable<Provider> GetProviders()
        {
            using (IDbConnection db = new NpgsqlConnection(ConnStr))
            {
                var SQL = "SELECT * FROM provider";
                var providers = db.Query<Provider>(SQL);
                return providers;
            }
            
        }

        public void GetTotalWeightAndTotalCost(int prt_id, int prv_id, string BeginDate, string EndDate, ref IEnumerable<WeightCost> total)
        {
            using (IDbConnection db = new NpgsqlConnection(ConnStr))
            {
                var SQL = $"SELECT SUM(dep_weight) AS weight, SUM(dep_price) AS cost FROM delivery_position WHERE prt_id = {prt_id} AND prv_id = {prv_id} AND dep_date > '{BeginDate}' AND dep_date < '{EndDate}'";
                total = db.Query<WeightCost>(SQL);
            }
        }
    }
}
