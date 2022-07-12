using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AlgoTraderDAL;

namespace AlgoTraderDAL
{
    public sealed class TDASettings
    {
        private readonly Entities db;
        private static readonly Lazy<TDASettings> lazy = new Lazy<TDASettings>(() => new TDASettings());
        public static TDASettings Instance { get { return lazy.Value; } }
        private TDASettings()
        {
            db = new Entities();
        }

        ~TDASettings()
        {
            db.Dispose();
        }

        public TDAKey GetDefaultKey()
        {
            TDAKey key = new TDAKey();
            try
            {
                key = db.TDAKeys.FirstOrDefault();
                if (key == null)
                {
                    key = new TDAKey();
                    key.last_update = DateTime.Now; 
                    db.TDAKeys.Add(key);
                    db.SaveChanges();
                }       
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to read from database: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return key;
        }

        public void UpdateDatabase()
        {
            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to Save TD Ameritrade Settings Data to database: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
