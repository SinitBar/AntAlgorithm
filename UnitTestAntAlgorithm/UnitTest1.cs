using NUnit.Framework;
using AntAlgorithm;
using System.IO;

namespace UnitTestAntAlgorithm
{
    partial class Test
    {
        static string directory = Directory.GetCurrentDirectory(); // извелакает текущее местоположение
        string input_filename = directory + "'\'input.txt"; // название файла с входными данными

        public void fill_input_file(string text) // вывод переданной строки в файл входных данных
        {
            using (StreamWriter sw = new StreamWriter(input_filename, false, System.Text.Encoding.UTF8))
            {
                sw.Write(text);
                sw.Close();
            }
        }

        [SetUp]
        public void Setup()
        {
            if (!File.Exists(input_filename)) // если файла с входными данными не существует, создается
                File.Create(input_filename);
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }

        [Test]
        public void TestGraphConstructor()
        { // проверка, что граф создается, и в него что-то записывается
            fill_input_file("A;B;10\nA;C;15\nB;C;20\nC;A;5");
            Graph G = new Graph(input_filename);
            Assert.AreEqual(G.is_empty(), false);
        }

        [Test]
        public void TestGraphConstructorVertexes()
        { // проверка, что вершины в граф записались верно
            fill_input_file("A;B;10\nA;C;15\nB;C;20\nC;A;5");
            Graph G = new Graph(input_filename);
            
            Assert.AreEqual(G.Vertexes.Count, 3);
            Assert.AreEqual(G.Vertexes[0].Name, "A");
            Assert.AreEqual(G.Vertexes[1].Name, "B");
            Assert.AreEqual(G.Vertexes[2].Name, "C");
        }

        [Test]
        public void TestGraphConstructorEdges()
        { // проверка, что ребра в граф записались верно
            fill_input_file("A;B;10\nA;C;15\nB;C;20\nC;A;5");
            Graph G = new Graph(input_filename);

            Assert.AreEqual(G.Vertexes[0].Edges.Count, 2);
            Assert.AreEqual(G.Vertexes[0].Edges[0].Name, "B");
            Assert.AreEqual(G.Vertexes[0].Edges[0].Distance, 10);
            Assert.AreEqual(G.Vertexes[0].Edges[1].Name, "C");
            Assert.AreEqual(G.Vertexes[0].Edges[1].Distance, 15);

            Assert.AreEqual(G.Vertexes[1].Edges.Count, 1);
            Assert.AreEqual(G.Vertexes[1].Edges[0].Name, "C");
            Assert.AreEqual(G.Vertexes[1].Edges[0].Distance, 20);

            Assert.AreEqual(G.Vertexes[2].Edges.Count, 1);
            Assert.AreEqual(G.Vertexes[2].Edges[0].Name, "A");
            Assert.AreEqual(G.Vertexes[2].Edges[0].Distance, 5);
        }

        [Test]
        public void TestGraphThreeVertexes()
        { // проверка, что ребра в граф записались верно
            fill_input_file("A;B;10\nA;C;15\nB;C;20\nC;A;5");
            Graph G = new Graph(input_filename);
            Assert.AreEqual(G.ant_algorithm(), true);
        }

        [Test]
        public void TestGraphThreeVertexes_2()
        { // проверка, что ребра в граф записались верно
            fill_input_file("A;B;10\nA;C;15\nB;C;20\nC;A;5\nC;B;5\nB;A;5");
            Graph G = new Graph(input_filename);
            Assert.AreEqual(G.ant_algorithm(), true);
        }

        [Test]
        public void TestGraphTwoVertexes()
        { 
            fill_input_file("A;B;10\nB;A;20");
            Graph G = new Graph(input_filename);
            Assert.AreEqual(G.ant_algorithm(), true);
        }

        [Test]
        public void TestGraphOneVertexes()
        { 
            fill_input_file(" ");
            Graph G = new Graph(input_filename);
            Assert.AreEqual(G.ant_algorithm(), false);
        }

        [Test]
        public void TestGraphOneVertexes_2()
        {
            fill_input_file("");
            Graph G = new Graph(input_filename);
            Assert.AreEqual(G.ant_algorithm(), false);
        }

        [Test]
        public void TestGraphTwoVertexesWrong_1()
        {
            fill_input_file("A;B;10\nA;C;5");
            Graph G = new Graph(input_filename);
            Assert.AreEqual(G.ant_algorithm(), false);
        }

        [Test]
        public void TestGraphTwoVertexesWrong_2()
        {
            fill_input_file("A;B;10\nB;A;-5");
            Graph G = new Graph(input_filename);
            Assert.AreEqual(G.ant_algorithm(), false);
        }

        [Test]
        public void TestGraph_1_10()
        {
            fill_input_file("A;B;2\nA;G;7\nA;F;8\nB;A;5\nB;G;2\nB;C;8\nC;B;3\nC;G;8\nC;D;6\nD;C;7\nD;G;6\nD;F;6\nF;D;2\nF;A;5\nG;A;6\nG;B;5\nG;C;3\nG;D;7\nG;F;6");
            Graph G = new Graph(input_filename);
            Assert.AreEqual(G.ant_algorithm(), true);
        }

        [Test]
        public void TestGraph_1_20()
        {
            fill_input_file("A;B;2\nA;G;7\nA;F;8\nB;A;5\nB;G;2\nB;C;8\nC;B;3\nC;G;8\nC;D;6\nD;C;7\nD;G;6\nD;F;6\nF;D;2\nF;A;5\nG;A;6\nG;B;5\nG;C;3\nG;D;7\nG;F;6");
            Graph G = new Graph(input_filename);
            Assert.AreEqual(G.ant_algorithm(20), true);
        }

        [Test]
        public void TestGraph_1_50()
        {
            fill_input_file("A;B;2\nA;G;7\nA;F;8\nB;A;5\nB;G;2\nB;C;8\nC;B;3\nC;G;8\nC;D;6\nD;C;7\nD;G;6\nD;F;6\nF;D;2\nF;A;5\nG;A;6\nG;B;5\nG;C;3\nG;D;7\nG;F;6");
            Graph G = new Graph(input_filename);
            Assert.AreEqual(G.ant_algorithm(50), true);
        }

        [Test]
        public void TestGraph_2_20() 
        {
            fill_input_file("A;B;8\nA;G;7\nA;F;9\nB;A;2\nB;G;2\nB;C;9\nC;B;2\nC;D;4\nD;C;2\nD;G;4\nD;F;7\nF;D;7\nF;G;7\nF;A;2\nG;A;8\nG;B;9\nG;C;2\nG;D;2");
            Graph G = new Graph(input_filename);
            Assert.AreEqual(G.ant_algorithm(20), true);
        }

        [Test]
        public void TestGraph_2_50()
        {
            fill_input_file("A;B;8\nA;G;7\nA;F;9\nB;A;2\nB;G;2\nB;C;9\nC;B;2\nC;D;4\nD;C;2\nD;G;4\nD;F;7\nF;D;7\nF;G;7\nF;A;2\nG;A;8\nG;B;9\nG;C;2\nG;D;2");
            Graph G = new Graph(input_filename);
            Assert.AreEqual(G.ant_algorithm(50), true);
        }
    }
    }