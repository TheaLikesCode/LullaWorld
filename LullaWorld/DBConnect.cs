using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using MySql.Data.MySqlClient;

namespace LullaWorld
{
    /**
   * Thea Marie Alnæs
   * Programmering 3 prosjekt
   * 30.05.2014
   */
    
    //Var brukt til high score. Velger å inkludere denne klassen som eksempel.
    internal class DBConnect
    {
        private MySqlConnection _connection;
        private string _server;
        private string _database;
        private string _uid;
        private string _password;

        private MySqlDataAdapter mySqlDataAdapter;

        public DBConnect()
        {
            Initialize();
        }

        private void Initialize()
        {
            //Kan ikke gi ut skolens DB info
        }

        //åpne forbindelse
        public bool OpenConnection()
        {
            try
            {
                _connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 0:
                        MessageBox.Show("Cannot connect to server.  Contact administrator");
                        break;

                    case 1045:
                        MessageBox.Show("Invalid username/password, please try again");
                        break;
                }
                return false;
            }
        }

        //Lukk forbindelse
        private bool CloseConnection()
        {
            try
            {
                _connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message, "Feilmelding");
                return false;
            }
        }


        public int GetPlayerId(string brukernavn)
        {
            if (!OpenConnection()) return 0;

            const string queryText = "SELECT ID FROM Bruker " +
                                     "WHERE Brukernavn = @Brukernavn";

            using (var cmd = new MySqlCommand(queryText, _connection))
            {
                cmd.Parameters.AddWithValue("@Brukernavn", brukernavn);
                MySqlDataReader dr = cmd.ExecuteReader();
                dr.Read();


                if (!dr.HasRows) return 0;

                int result = Convert.ToInt16(dr["ID"].ToString());
                CloseConnection();
                return result;
            }
        }


        public Int32 GetPersonalBest(int id)
        {
            if (!OpenConnection()) return 0;
            const string queryText = "SELECT MAX(score) as maxscore FROM HighScore " +
                                     "WHERE brukerId = @ID";

            using (var cmd = new MySqlCommand(queryText, _connection))
            {
                cmd.Parameters.AddWithValue("@ID", id); // cmd is SqlCommand 
                MySqlDataReader dr = cmd.ExecuteReader();


                if (!dr.HasRows) return 0;

                dr.Read();
                int result = Convert.ToInt32(dr["maxscore"].ToString());
                CloseConnection();
                return result;
            }
            return 0;
        }


        public bool CheckPassword(string brukernavn, string passord)
        {
            if (!OpenConnection()) return false;

            const string queryText = "SELECT * FROM Bruker " +
                                     "WHERE Brukernavn = @Brukernavn AND Passord = @Passord";


            using (var cmd = new MySqlCommand(queryText, _connection))
            {
                cmd.Parameters.AddWithValue("@Brukernavn", brukernavn); 
                cmd.Parameters.AddWithValue("@Passord", passord);
                MySqlDataReader dr = cmd.ExecuteReader();
        

                if (!dr.HasRows) return false;

                CloseConnection();

                return true;
            }
        }


        public List<int> GetHighScores()
        {
            /* if (!OpenConnection()) return null;

             const string queryText = "SELECT HighScore FROM Bruker ORDER BY HighScore DESC";
             var listToReturn = new List<int>();
             using (var cmd = new MySqlCommand(queryText, connection))
             {
                 // cmd.Parameters.AddWithValue("@ID", id); // cmd is SqlCommand 
                 // int result = (int)cmd.ExecuteScalar();
                 MySqlDataReader dr = cmd.ExecuteReader();

                 if (dr.HasRows)
                 {
                     while (dr.Read())
                     {
                         listToReturn.Add(Convert.ToInt16(dr["HighScore"].ToString()));
                     }
                     CloseConnection();
                     return listToReturn;
                 }
             }*/
            return null;
        }


        public void InsertScore(int _score, int _id)
        {
            String insertQ = String.Format("INSERT INTO HighScore(score, brukerID) VALUES({0}, {1})", _score, _id);

            if (!OpenConnection()) return;

            try
            {
                var updateCommand = new MySqlCommand(insertQ, _connection);
                updateCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                MessageBox.Show("Feil med oppdatering av DB: " + e.Message);
            }

            CloseConnection(); 
        }
    }
}
