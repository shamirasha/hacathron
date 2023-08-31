using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Configuration;


namespace hacathonepractice
{
    [Serializable]
    public class PatientRecordException : Exception
    {
        public PatientRecordException() { }
        public PatientRecordException(string message) : base(message) { }
        public PatientRecordException(string message, Exception inner) : base(message, inner) { }
        protected PatientRecordException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
    interface IPatient
    {
        int Id { get; set; }
        string Name { get; set; }
        int Age { get; set; }
        string Disease { get; set; }
    }
    [Serializable]
    class Patient : IPatient
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Disease { get; set; }
    }
    class PatientFactory
    {
        public static IPatient CreatePatient()
        {
            return new Patient();
        }
    }
    class practice2
    {
        static Dictionary<int, IPatient> patientRecords = new Dictionary<int, IPatient>();

        private static readonly string fileName = ConfigurationManager.AppSettings["MenuFile"];
        static string content = File.ReadAllText(fileName);
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine(content);

                //Console.WriteLine("Patient Record Management System");
                //Console.WriteLine("1. Add Patient");
                //Console.WriteLine("2. View Patient");
                //Console.WriteLine("3. Update Patient");
                //Console.WriteLine("4. Delete Patient");
                //Console.WriteLine("5. Serialize Patient Records");
                //Console.WriteLine("6. Deserialize Patient Records");
                //Console.WriteLine("7. Exit");
                //Console.Write("Enter your choice: ");

                int choice;
                if (!int.TryParse(Console.ReadLine(), out choice))
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                    continue;
                }

                try
                {
                    switch (choice)
                    {
                        case 1:
                            AddPatient();
                            break;
                        case 2:
                            ViewPatient();
                            break;
                        case 3:
                            UpdatePatient();
                            break;
                        case 4:
                            DeletePatient();
                            break;
                        case 5:
                            SerializePatientRecords();
                            break;
                        case 6:
                            DeserializePatientRecords();
                            break;
                        case 7:
                            Console.WriteLine("Exiting the program.");
                            return;
                        default:
                            Console.WriteLine("Invalid choice. Please select a valid option.");
                            break;
                    }
                }
                catch (PatientRecordException ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
        }

        static void AddPatient()
        {
            Console.Write("Enter Patient ID: ");
            int id = int.Parse(Console.ReadLine());

            if (patientRecords.ContainsKey(id))
            {
                throw new PatientRecordException("Patient with this ID already exists.");
            }

            IPatient patient = PatientFactory.CreatePatient();
            patient.Id = id;

            Console.Write("Enter Patient Name: ");
            patient.Name = Console.ReadLine();

            Console.Write("Enter Patient Age: ");
            patient.Age = int.Parse(Console.ReadLine());

            Console.Write("Enter Patient Disease: ");
            patient.Disease = Console.ReadLine();

            patientRecords.Add(id, patient);
            Console.WriteLine("Patient added successfully.");
        }

        static void ViewPatient()
        {
            Console.Write("Enter Patient ID to view: ");
            int id = int.Parse(Console.ReadLine());

            if (patientRecords.ContainsKey(id))
            {
                IPatient patient = patientRecords[id];
                Console.WriteLine($"ID: {patient.Id}, Name: {patient.Name}, Age: {patient.Age}, Disease: {patient.Disease}");
            }
            else
            {
                Console.WriteLine("Patient not found.");
            }
        }

        static void UpdatePatient()
        {
            Console.Write("Enter Patient ID to update: ");
            int id = int.Parse(Console.ReadLine());

            if (patientRecords.ContainsKey(id))
            {
                IPatient patient = patientRecords[id];

                Console.Write("Enter new Patient Name: ");
                patient.Name = Console.ReadLine();

                Console.Write("Enter new Patient Age: ");
                patient.Age = int.Parse(Console.ReadLine());

                Console.Write("Enter new Patient Disease: ");
                patient.Disease = Console.ReadLine();

                Console.WriteLine("Patient updated successfully.");
            }
            else
            {
                Console.WriteLine("Patient not found.");
            }
        }

        static void DeletePatient()
        {
            Console.Write("Enter Patient ID to delete: ");
            int id = int.Parse(Console.ReadLine());

            if (patientRecords.ContainsKey(id))
            {
                patientRecords.Remove(id);
                Console.WriteLine("Patient deleted successfully.");
            }
            else
            {
                Console.WriteLine("Patient not found.");
            }
        }

        static void SerializePatientRecords()
        {
            using (FileStream fs = new FileStream("patient_records.dat", FileMode.Create))
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fs, patientRecords);
            }

            Console.WriteLine("Patient records serialized successfully.");
        }

        static void DeserializePatientRecords()
        {
            if (File.Exists("patient_records.dat"))
            {
                using (FileStream fs = new FileStream("patient_records.dat", FileMode.Open))
                {
                    IFormatter formatter = new BinaryFormatter();
                    patientRecords = (Dictionary<int, IPatient>)formatter.Deserialize(fs);
                }

                Console.WriteLine("Patient records deserialized successfully.");
            }
            else
            {
                Console.WriteLine("Patient records file not found.");
            }
        }
    }


}
