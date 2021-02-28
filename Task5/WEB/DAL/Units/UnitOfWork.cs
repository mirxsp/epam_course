using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEB.DAL.Contexts;
using WEB.DAL.Repositories;
using WEB.Models;

namespace WEB.DAL.Units
{
    public class UnitOfWork : IDisposable
    {
        private SalesContext context;
        public GenericRepository<Client> ClientRepository { get; set; }
        public GenericRepository<Item> ItemRepository { get; set; }
        public GenericRepository<Sale> SaleRepository { get; set; }
        public GenericRepository<Manager> ManagerRepository { get; set; }

        public event Action<string> Log 
        { 
            add
            {
                context.Database.Log += value;
            }
            remove
            {
                context.Database.Log -= value;
            }
        }

        public UnitOfWork()
        {
            context = new SalesContext();
            ClientRepository = new GenericRepository<Client>(context);
            ItemRepository = new GenericRepository<Item>(context);
            SaleRepository = new GenericRepository<Sale>(context);
            ManagerRepository = new GenericRepository<Manager>(context);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    context.Dispose();
                }
                disposedValue = true;
            }
        }

        ~UnitOfWork()
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
