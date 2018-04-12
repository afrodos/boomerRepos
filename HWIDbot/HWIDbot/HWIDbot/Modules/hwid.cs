using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using MySql.Data.MySqlClient;

namespace HWIDbot.Modules
{
    public class hwid : ModuleBase<SocketCommandContext>
    {
        [Command("hwid")]
        public async Task hwidAsync([Remainder]string hwid)
        {
            MySqlConnection conn;
            string server;
            string database;
            string uid;
            string password;
            string connstring;

            server = "sql2.freemysqlhosting.net";
            database = "sql2231671";
            uid = "sql2231671";
            password = "wK5*lI7!";

            connstring = $"SERVER={server};DATABASE={database};UID={uid};PASSWORD={password};";
            conn = new MySqlConnection(connstring);

            bool OpenConnection()
            {
                try
                {
                    conn.Open();
                    return true;
                }
                catch (MySqlException ex)
                {
                    switch (ex.Number)
                    {
                        case 0:
                            break;
                        case 1045:
                            ReplyAsync("ERROR");
                            break;
                    }
                    return false;
                }
            }

            bool Register(string hwidpass)
            {
                string query = $"INSERT INTO users (id, hwid, discordName) VALUES ('','{hwidpass}', '{Context.User.Mention}');";

                try
                {
                    if (OpenConnection())
                    {
                        MySqlCommand cmd = new MySqlCommand(query, conn);
                        try
                        {
                            cmd.ExecuteNonQuery();
                            return true;
                        }
                        catch (Exception ex)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        conn.Close();
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    conn.Close();
                    return false;
                }
            }

            if (!(hwid.Length <= 13 || hwid.Length >= 19))
            {
                string hwidpass = hwid;

                if (Register(hwidpass))
                {
                    ReplyAsync($"User {hwid} has been added for {Context.User.Mention}");
                }
                else
                {
                    ReplyAsync("ERROR");
                }
            }
            else
            {
                ReplyAsync($"{Context.User.Mention} that is a invalid hwid!");
            }
        }
    }
}
