using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace appli_bancaire
{
    class Program
    {
        static void Main(string[] args)
        {
            Comptecourant compteNicolas = new Comptecourant(2000);
            compteNicolas.Propriétaire = "nicolas";
            CompteEpargne compteEpargneNicolas = new CompteEpargne(0.02);
            compteEpargneNicolas.Propriétaire = "nicolas";
            Comptecourant compteJeremie = new Comptecourant(500);
            compteNicolas.Propriétaire = "jeremie";
            compteNicolas.Créditer(100);
            compteNicolas.Debiter(50);
            compteEpargneNicolas.Créditer(20, compteNicolas);
            compteEpargneNicolas.Créditer(100);
            compteEpargneNicolas.Debiter(20,compteNicolas);
            compteJeremie.Debiter(500);
            compteJeremie.Debiter(200,compteNicolas);
            Console.WriteLine("solde du compte courant de Nicolas : " + compteNicolas);
            Console.WriteLine("solde du compte épargne de Nicolas : " + compteEpargneNicolas);
            Console.WriteLine("solde du compte courant de Jeremie : " + compteJeremie);       
            compteNicolas.AfficheResume();

            Console.ReadKey();
        }
    }
    //	Définir une énumération publique nommée Mouvement 
    public enum Mouvement
    {
        Credit,
        Debit
    }
    //	Définir une classe publique nommée Opération 
    public class Opération
    {
        #region typedemouvement
        private Mouvement typedemouvement;
        public Mouvement TypeDeMouvement
        {
            get
            {
                return typedemouvement;
            }
            set
            {
                typedemouvement = value;
            }
        }
        #endregion
        //propriété publique Montant de type decimal et ses accesseurs 
        #region Montant
        private decimal montant;
        public decimal Montant
        {
            get
            {
                return montant;
            }
            set
            {
                montant = value;
            }
        }
        #endregion
        public Opération(Mouvement typedemouvementrecu, decimal montantrecu)
        {
            typedemouvement = typedemouvementrecu;
            montant = montantrecu;
        }
    }
    //Définir une classe publique et abstraite  nommée Compte 
    public abstract class Compte
    {
        #region propriété
        //propriété publique Propriétaire 
        private string propriétaire;
        public string Propriétaire
        {
            get
            {
                return propriétaire;
            }
            set
            {
                propriétaire = value;
            }
        }
        #endregion
        protected List<Opération> listeOperations;
        //constructeur
        public Compte()
        {
            listeOperations = new List<Opération>();
        }
        // méthode publique Créditer
        #region Créditer
        public void Créditer(decimal montant)
        {
            //puis ajoute l'objet operation à la liste listeOperations (méthode Add)
            Opération operation = new Opération(Mouvement.Credit, montant);
            listeOperations.Add(operation);
        }

        //méthode publique Créditer() qui ne retourne rien 
        public void Créditer(decimal montant, Compte Compte)
        {
            this.Créditer(montant);
            Compte.Debiter(montant);
        }
        #endregion
        //méthode publique Debiter() qui ne retourne rien (Void)
        public void Debiter(decimal montant)
        {
            Opération operation = new Opération(Mouvement.Debit, montant);
            listeOperations.Add(operation);
        }
        //méthode publique Debiter() 
        public void Debiter(decimal montant, Compte Compte)
        {
            this.Debiter(montant);
            Compte.Créditer(montant);
        }
        //propriété publique Solde héritable (mot clé virtual)
        #region solde héritable

        public virtual decimal SoldeHeritable
        {
            get
            {
                decimal total = 0;
                foreach (Opération operation in listeOperations)
                {
                    if (operation.TypeDeMouvement == Mouvement.Credit)
                        total += operation.Montant;
                    else
                        total -= operation.Montant;
                }
                return total;
            }
        }
        #endregion

        protected void AfficheOperations()
        {
            Console.WriteLine("Opérations :");
            foreach (var item in listeOperations)
            {
                if (item.TypeDeMouvement == Mouvement.Credit)
                    Console.WriteLine("+" + item.Montant);
                else
                    Console.WriteLine("-" + item.Montant);
            }


        }
        public virtual void AfficheResume()
        {

        }
    }

    public class Comptecourant : Compte
    {
        private decimal decouvert;

        public Comptecourant(decimal decouvertAutorise)
        {
            this.decouvert = decouvertAutorise;
        }
        public override void AfficheResume()
        {

            Console.WriteLine("le propriaitaire du compte est : " + this.Propriétaire);
            Console.WriteLine("le solde du compte est : " + this.SoldeHeritable);
            Console.WriteLine("le decouvert autorise est  de  : " + this.decouvert);
            this.AfficheOperations();


        }

    }



    public class CompteEpargne : Compte
    {

        private double tauxAbondement;
        public CompteEpargne(double tauxcreation)
        {

            this.tauxAbondement = tauxcreation;
        }

        public override void AfficheResume()
        {
            Console.WriteLine("le propriaitaire du compte est : " + this.Propriétaire);
            Console.WriteLine("le solde du compte est : " + this.SoldeHeritable);
            Console.WriteLine("le taux abondement du compte est : " + this.tauxAbondement);
            Console.WriteLine("le operation sont les suivante  : ");
            this.AfficheOperations();


        }
        public override decimal SoldeHeritable
        {
            get
            {

                decimal compteavectaux = (base.SoldeHeritable * (decimal)(1 + tauxAbondement));
                return compteavectaux;
            }
        }








    }
}

