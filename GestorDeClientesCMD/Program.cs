using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace GestorDeClientesCMD
{
    class Program
    {
        [System.Serializable]
        struct Cliente
        {
            public string nome;
            public string email;
            public string cpf;
        }

        static List<Cliente> clientes = new List<Cliente>();
        enum Menu { Listagem = 1, Adicionar = 2, Remover = 3, Sair = 4 }
        static void Main(string[] args)
        {
            Carregar();
            bool escolheuSair = false;
            while (!escolheuSair)
            {
                Console.WriteLine("Sistema de clientes - Bem vindo!");
                Console.WriteLine();
                Console.WriteLine("1 - Listagem\n2 - Adicionar\n3 - Remover\n4 - Sair");
                int intOp = int.Parse(Console.ReadLine());
                Menu opcao = (Menu)intOp;

                switch (opcao)
                {
                    case Menu.Adicionar:
                        Adicionar();
                        break;

                    case Menu.Listagem:
                        Listagem();
                        break;

                    case Menu.Remover:
                        Remover();
                        break;

                    case Menu.Sair:
                        escolheuSair = true;
                        break;
                }

                Console.Clear();
            }
        }

        static void Adicionar()
        {
            Console.Clear();
            Cliente client = new Cliente();
            Console.WriteLine("Cadastro de cliente: ");
            Console.WriteLine();
            Console.Write("Nome do cliente: ");
            client.nome = Console.ReadLine();

            Console.Write("Email do cliente: ");
            client.email = Console.ReadLine();

            Console.Write("CPF do cliente: ");
            client.cpf = Console.ReadLine();

            clientes.Add(client);
            Salvar();

            Console.WriteLine();
            Console.WriteLine("Cadastro concluido, aperte ENTER para voltar ao menu.");
            Console.ReadLine();

        }

        static void Listagem(bool pausar = true)
        {
            Console.Clear();

            if (clientes.Count > 0)
            {
                Console.WriteLine("Lista de clientes: ");
                Console.WriteLine();
                int i = 0;

                foreach (Cliente cliente in clientes)
                {
                    Console.WriteLine($"ID: {i}");
                    Console.WriteLine($"Nome: {cliente.nome}");
                    Console.WriteLine($"Email: {cliente.email}");
                    Console.WriteLine($"CPF: {cliente.cpf}");
                    Console.WriteLine("==========================");
                    i++;
                }
            }
            else
            {
                Console.WriteLine("Nenhum cliente cadastrado!");
            }
            Console.WriteLine();
            if (pausar)
            {
                Console.WriteLine("Aperte ENTER para voltar ao menu.");
                Console.ReadLine();
            }
        }

        static void Remover()
        {
            Listagem(false);
            Console.WriteLine("Digite o ID do cliente que você quer remover: ");
            string input = Console.ReadLine();
            if (int.TryParse(input, out int id) && id >= 0 && id < clientes.Count)
            {
                clientes.RemoveAt(id);
                Console.WriteLine("Cliente removido com sucesso!");
                Salvar();

                Console.WriteLine();
                Console.WriteLine("Aperte ENTER para voltar ao menu.");
            }
            else
            {
                Console.WriteLine("ID inválido ou entrada inválida!");
            }
            Console.ReadLine();

        }

        static void Salvar()
        {
            using (FileStream stream = new FileStream("clients.dat", FileMode.OpenOrCreate))
            {
                BinaryFormatter encoder = new BinaryFormatter();
                encoder.Serialize(stream, clientes);
            }
        }


        static void Carregar()
        {
            FileStream stream = new FileStream("clients.dat", FileMode.OpenOrCreate);
            try
            {

                BinaryFormatter enconder = new BinaryFormatter();

                clientes = (List<Cliente>)enconder.Deserialize(stream);

                if (clientes == null)
                {
                    clientes = new List<Cliente>();
                }
            }
            catch (Exception e)
            {
                clientes = new List<Cliente>();
            }

            stream.Close();
        }
    }
}
