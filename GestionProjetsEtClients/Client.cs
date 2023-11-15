using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionProjetsEtClients
{
    internal class Client
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Adresse { get; set; }
        public string NoTelephone { get; set; }
        public string Email { get; set; }

        public override string ToString()
        {
            return $"Id = {Id}, Nom = {Nom}, Adresse = {Adresse}, NoTelephone = {NoTelephone}, Email = {Email}";
        }
    }
}
