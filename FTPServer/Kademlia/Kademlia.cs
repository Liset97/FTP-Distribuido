using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Net;
using System.Text;
using System.Net.Sockets;
using System.Numerics;


namespace Kademlia
{
    public enum PingMensage {  PONG }

    public class Contact
    {
     
        public BigInteger nodeID { get; set; }
        public IPAddress IPAddress { get; set; }
        public UdpClient Port { get; set; }
        public Protocol Prot { get; set; }
        
        public Contact( IPAddress IPAddress, UdpClient Port)
        {
            Encriptar();
            this.IPAddress = IPAddress;
            this.Port = Port;
        }

        public void Encriptar()
        {
            byte[] b = Encoding.UTF8.GetBytes(IPAddress.ToString() + Port.ToString());
            SHA1 funtion = new SHA1CryptoServiceProvider();
            byte[] id = funtion.ComputeHash(b);
            BigInteger auau = BitConverter.ToUInt64(id, 0);
            //Esta conversion en realidad no sirve de mucho xq es en base a 64, la idea seria 
            //en base a 160 
            //Asignarle valor a nodeID, despues de ver como convertir id en un BigInteger 
        }


        public override bool Equals(object obj)
        {
            if(obj is Contact)
            {
                Contact aux = (Contact)obj;
                if (nodeID == aux.nodeID) return true;
            }
            return false;
        }

        public override string ToString()
        {
            return "Contact with ID: "+nodeID.ToString();
        }


    }

    public class Bucket
    {
        public int j { get; set; }
        public List<Contact> C { get; set; }
        public int k { get; set; }
        public BigInteger Low { get; set; }
        public BigInteger High { get; set; }


        public Bucket(int k)
        {
            this.k = k;
            C = new List<Contact>();
            j = 0;
            Low = 0;
            High = BigInteger.Pow(2, 160);
        }

        public Bucket(BigInteger l,BigInteger h, int k, int j)
        {
            this.j = j;
            this.k = k;
            C = new List<Contact>();
            Low = l;
            High = h;
        }

        /// <summary>
        /// Retorna true si el Contacto a se encuentra en el Bucket, falso en caso contrario
        /// </summary>
        public bool Contains(Contact a)
        {
            foreach (var item in C)
            {
                if (item.nodeID == a.nodeID) return true;
            }
            return false;
        }

        /// <summary>
        /// Si el Bucket no esta lleno y no se encuentra el Contacto a, entonces lo añade al bucket
        /// </summary>
        public void Add(Contact a)
        {
            if(C.Count<k && !Contains(a))
            {
                C.Add(a);
            }
        }

        /// <summary>
        /// Si el Contacto a se encuentra en el Bucket lo elimina de la lista
        /// </summary>
        public void Remove(Contact a)
        {
            if (Contains(a))
            {
                C.Remove(a);
            }
        }

        /// <summary>
        /// Retorna true si el Contacto a se encuentra en el rango del Bucket
        /// </summary>
        public bool Range(Contact a)
        {
            if(a.nodeID>=Low && a.nodeID<High)
            {
                return true;
            }
            return false;
        }

        public bool Range(BigInteger a)
        {
            if (a >= Low && a < High)
            {
                return true;
            }
            return false;
        }

        #region Split
        /// <summary>
        /// Supuestamente este metodo lo que realiza es dividir un Bucket en 2, 
        /// dejando en el actual los valores menores que el valor medio del rango
        /// y devuelve otro Bucket en con rango el valor medio y el valor mayor anteriores 
        /// del bucket
        /// </summary>
        /// <returns></returns>
        //public Bucket Split()
        //{
        //    BigInteger m = (Low + High) / 2;
        //    Bucket result = new Bucket(m, High);
        //    BigInteger aux = High;

        //    High = m;

        //    foreach (var item in C)
        //    {
        //        if (item.nodeID >= m)
        //        {
        //            result.Add(item);
        //            Remove(item);

        //        }
        //    }
        //    return result;
        //}
        #endregion


    }

    public class TablePath
    {
        public int k { get; set; }
        public List<Bucket> Buckets { get; set; }
        public BigInteger nodeID { get; set; }
        public Contact contact { get; set; }

        public DateTime WaitPing { get; set; }

        public TablePath(int k, Contact contact)
        {
            this.k = k;
            nodeID = contact.nodeID;
            Buckets = new List<Bucket>();
            for (int i = 0; i < 160; i++)
            {
                Buckets.Add(new Bucket(BigInteger.Pow(2, i), BigInteger.Pow(2, i + 1), k, i));
            }
            this.contact = contact;
        }


        /// <summary>
        /// Terminar!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        /// Este metodo es para adicionar un Contacto a la tabla de encadenamiento,
        /// a menos que sea el propio contacto q tiene la tabla o si el contacto ya esta
        /// en la misma
        /// </summary>
        public void Add(Contact a)
        {
            if (a.nodeID == nodeID)
            {
                //Console.WriteLine("");
                return;    
            }

            foreach (var item in Buckets)
            {
                if (item.Range(a))
                {
                    if (item.C.Count < k)
                    {
                        // Ver como a la hora de agregarlo hacer el XOR
                        item.Add(a);
                    }
                    else
                    {
                        foreach (var cont in item.C)
                        {
                            ///
                            /// Aqui se verifica si alguno los contactos que pertenecen al bucket no esta alcanzable,
                            /// es decir, que su informacion fallo o algo x el estilo.
                            /// Esta verivicacion se realiza por el protocolo PING, donde se espera 
                            /// un tiempo a ver si el contacto responde PONG. Si lo hace se pasa a verificar
                            /// los siguientes contactos. Si alguno se demora mas del tiempo que se determine, 
                            /// entonces se remueve del bucket que se esta analizando y se agrega el contacto que se
                            /// queria iuntroducir de contacto.
                            /// Si por el contrario, todos los contactos del bucket respondieron PONG en el tiempo determinado
                            /// entonces el nuevo contacto no se agrega.
                            ///
                        }
                    }
                }
            }

        }


        /// <summary>
        /// Este metodo busca en la tabla de encaminamiento(la lista de Buckets), el buket donde el contacto esta en 
        /// el rango de ids
        /// </summary>
        public Bucket LookBucket(Contact a)
        {
            foreach (var item in Buckets)
            {
                if (item.Range(a)) return item;
            }
            return new Bucket(k);
        }



    }

    public  class Protocol
    {
        public Node Node { get; set; }

        public PingMensage pm { get; set; }

        // PING
        public PingMensage Ping(Contact a)
        {
            pm = Node.Ping(a);
            return pm;
        }

        // FindNode
        public List<Contact> FindNode(BigInteger id)
        {
            List<Contact> result = Node.FindNode(id);
            return result;
        }

        // FindValue
        // Store

    }

    public class Node
    {
        public BigInteger ID { get; set; }
        public Contact proper { get; set; }
        public TablePath TPath { get; set; }
        public int k { get; set; }

        //Ver como agregarle lo de la base de datos....que  ni puta idea
        ///     Para inicializar la clase nodo(los servidores de la red), seria necesario
        ///     inicializarlo con una base de datos. O otra alternativa seria ideal una maquinaria
        ///     para la introduccion paulatina de la informacion en la base de datos sin necesidad
        ///     de inicializar el nodo directamente con ella
        /// 
        
        


        public Node(Contact contact)
        {
            proper = contact;
            ID = contact.nodeID;
        }

        public PingMensage Ping(Contact a)
        {
            if (a.nodeID == proper.nodeID) { }
            return PingMensage.PONG;
        }

        /// <summary>
        /// Este es el FINDNODE que devuelve la lista de contactos a los cuales esta cercano un id,
        /// es decir, Cuando le hacen FindNode este es el metodo que se utiliza como respuesta.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<Contact> FindNode(BigInteger id)
        {
            List<Contact> contacts = new List<Contact>();
            var bucket = TPath.Buckets.Find(b => b.Range(id));
            return bucket.C;
        }

        /// <summary>
        /// Este es el FINDNODE que se utiliza para dado que ya conoces a un contacto a, 
        /// pedirle a este que te devuelva los k mas cercanos a el mediante su propio id
        /// o mediante el id de otro contacto
        /// </summary>
        /// <param name="a"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<Contact> MakeFindNode(Contact a, BigInteger id)
        {
            List<Contact> contacts = new List<Contact>();
            var aux = TPath.Buckets.Find(b => b.Range(a)).Contains(a);
            if (aux)
            {
                //Ni idea de como hacer una coneccion con el!!!!!!!!!!!!!!!!!
                contacts = a.Prot.FindNode(id);                         
            }

            return contacts;
        }

        #region Aki esta explicado los dos protocolos que faltan por implementar aki y en la clase Protocol
        //1. FindValue -------- para buscar un un archivo dada una llave, es decir, buscar en la 
        //                      TablePath el contacto que mas cercano este a la tabla:
        ///     El nodo que realiza la llamada debe incluir un identificador de 160 bits, que coincidira
        ///     en caso de existencia con la clave de la tabla de datos de algun nodo, que, en caso de coincidir
        ///     este sera el nodo con el identificador mas cercano a la clave
        ///     El nodo destino, si tiene en su tabla esta clave, devolvera el vallor asociado en caso de que no
        ///     contenga esa clave dentro de su tabla de informacion, devolvera k contactos cercanos a la clave

        //2. Store     -------- Envia a un contacto para que guarde una informacion conformado
        //                      por el par clave/valor
        ///     Esta llamada a procedimiento remoto est'a destinada al almacenamiento de la informacion
        ///     relativa a los datos que pudiera requerir un emisor y no a los datos de configuracion
        ///     o conocimiento sobre la red.
        ///     El nodo que genera la llamada debe de incluir la clave y el valor, que el propio nodo que 
        ///     recibe la llamada, almacenara en su tabla de datos parta si alguien preguntase por la clave,
        ///     pudiera indicarle el valor correspondiente.
        /// 
        #endregion



    }


}
