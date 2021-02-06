using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task4.DAL.Units;
using Task4.Model;

namespace Task4.Committers.BL
{
    public class DataCommiter : IDisposable
    {
        private UnitOfWork unit;
        private bool disposedValue;

        public event Action<string> Log;
        public event Action<string> LogDB
        {
            add
            {
                unit.Log += value;
            }
            remove
            {
                unit.Log -= value;
            }
        }

        public DataCommiter()
        {
            unit = new UnitOfWork();
        }

        public DataCommiter(Action<string> LogFunc)
        {
            Log += LogFunc;
            //Log?.Invoke("Creating UOF");
            unit = new UnitOfWork();
            //Log?.Invoke("Created UOF");
            LogDB += LogFunc;
        }

        public virtual bool Push(Sale sale)
        {
            bool dbIsModified = false;
            Log?.Invoke("Checking for existing client in DB...");
            Client client = null;
            try
            {
                client = unit.ClientRepository.Get(x => x.Name == sale.Client.Name).FirstOrDefault();
            }
            catch (Exception e)
            {
                Log?.Invoke("Failed to get client from DB." + e.Message);
                return false;
            }
            if (client==null)
            {
                Log?.Invoke("Client not found. Adding client to DB...");
                client = sale.Client;
                unit.ClientRepository.Add(client);
                dbIsModified = true;
            }
            else
            {
                Log?.Invoke("Client is found.");
            }
            Log?.Invoke("Checking for existing item in DB...");
            Item item = null;
            try
            {
                item = unit.ItemRepository.Get(x => x.Name == sale.Item.Name && x.Price == sale.Item.Price).FirstOrDefault();
            }
            catch(Exception e)
            {
                Log?.Invoke("Failed to get item from DB." + e.Message);
                return false;
            }
            if (item==null)
            {
                Log?.Invoke("Item not found. Adding item to DB...");
                item = sale.Item;
                unit.ItemRepository.Add(item);
                dbIsModified = true;
            }
            else
            {
                Log?.Invoke("Item is found.");
            }
            if (dbIsModified)
            {
                try
                {
                    unit.Save();
                    Log?.Invoke("DB modificated successfully.");
                }
                catch(DbUpdateException)
                {
                    unit.ItemRepository.Remove(item);
                    unit.ClientRepository.Remove(client);
                    Log?.Invoke("DB Modification Error. Invalid item or client.");
                    return false;
                }
            }
            sale.Client = client;
            sale.Item = item;
            unit.SaleRepository.Add(sale);
            try
            {
                unit.Save();
                Log?.Invoke("DB modificated successfully.");
            }
            catch (DbUpdateException)
            {
                unit.SaleRepository.Remove(sale);
                Log?.Invoke("DB Modification Error. Invalid sale.");
                return false;
            }
            return true;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    unit.Dispose();
                }
                disposedValue = true;
            }
        }

        ~DataCommiter()
        {
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
