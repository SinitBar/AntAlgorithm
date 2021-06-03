using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("UnitTestAntAlgorithm")] // добавляем проекту теста работать с именем AntAlgorithm

namespace AntAlgorithm
{
    class Graph // для создания графа необходимо наличие хотя бы 2 вершин
    {
        public List<VertexWithEdges> Vertexes { get; } // граф хранится в виде списка вершин, в каждой из которых хранится список исходящих из нее ребер

        static Random random = new Random();
        public Graph()
        {
            Vertexes = new List<VertexWithEdges>(); // список вершин, каждая из которых содержит список исходящих из нее ребер
        }

        public Graph(string input_filename)
        { // конструктор объекта типа Graph, направленные ребра графа считываются из файла
            // формат задания данных: одна строка описывает информацию об 1 направленном ребере
            // строка должна иметь формат: Имя_начальной_вершины;Расстояние(вес ребра);Имя_конечной_вершины\n
            Vertexes = new List<VertexWithEdges>();
            try
            {
                int lines = 0;
                using (StreamReader reader = new StreamReader(input_filename, System.Text.Encoding.UTF8))
                {
                    try
                    {
                        string line;
                        
                        while ((line = reader.ReadLine()) != null) // файл считывается построчно
                        {
                            lines++;
                            int count_delimeter = line.Where(c => c == ';').Count(); // подсчет количества ; в строке

                            if (count_delimeter != 2) // в строке должно быть по 2 разделителя, чтобы было три подстроки
                            {
                                if (count_delimeter == 0)
                                {
                                    if (line == "" || line.Trim() == string.Empty)
                                    {
                                        Console.WriteLine("Невозможно создать пустой граф");
                                        throw new Exception("Необходимые входные данные отсутствуют");
                                    }
                                    Console.WriteLine("Граф состоит из одной вершины и автоматически гамильтонов (такой граф называется вершиной)");
                                    throw new ArgumentOutOfRangeException("Исходный файл содержит единственную вершину, чего недостаточно для создания графа, одна вершина считается вершиной, а не графом");
                                }
                                Console.WriteLine("В одной или нескольких строках файла недостаточно или слишком много разделителей");
                                throw new ArgumentOutOfRangeException("Исходный файл содержит неверное количество разделителей");
                            }

                            string[] substring = { "", "", "" }; // задаем три подстроки для каждой строки
                            substring = line.Split(';');

                            if (substring[0] == "" || substring[1] == "" || substring[2] == "") // если какая-то подстрока не заполнена, то это ошибка в исходных данных
                            {
                                Console.WriteLine("В одной или нескольких строках файла отсутствуют необходимые данные");
                                throw new ArgumentOutOfRangeException("Недостаточно исходных данных");
                            }

                            int index = search_index_of_vertex(substring[0]); // первая подстрока - название вершины, откуда исходит ребро
                            EdgeEnd new_edge;
                            if (double.TryParse(substring[2], out double distance)) // третья подстрока - длина пути из начальной вершины в конечную
                                new_edge = new EdgeEnd(substring[1], distance); // вторая подстрока - название вершины, куда приводит ребро
                            else
                            {
                                Console.WriteLine("В одной или нескольких строках файла расстояние записано в формате, отличном от double");
                                throw new ArgumentOutOfRangeException("Файл содержит расстояние в некорректном формате");
                            }
                            if (distance <= 0)
                            {
                                Console.WriteLine("Отрицательные и равные нулю расстояния (веса ребер) в данной задаче не предусмотрены");
                                throw new ArgumentOutOfRangeException("Файл содержит отрицательное или нулевое расстояние");
                            }
                            if (index < 0)
                            {
                                VertexWithEdges new_vertex = new VertexWithEdges(substring[0]);
                                new_vertex.Edges.Add(new_edge);
                                Vertexes.Add(new_vertex);
                            }
                            else
                                Vertexes[index].Edges.Add(new_edge);
                        }
                    }
                    catch (ArgumentOutOfRangeException e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    reader.Close();
                }
                if (lines == 0)
                {
                    Console.WriteLine("Отсутствуют необходимые входные данные");
                    throw new Exception("Исходный файл пуст");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Файл не был открыт:");
                Console.WriteLine(e.Message);
            }
        }

        public bool is_empty()
        {
            if (Vertexes != null)
            {
                if (Vertexes.Count > 0)
                    return false; // так как граф не пуст, возвращаемое значение - ложь
            }
            return true;
        }

        void clear_marks() // очищает пометки о посещении у всех вершин графа
        {
            if (!is_empty())
            {
                foreach (VertexWithEdges vertex in Vertexes)
                    vertex.IsVisited = false;
            }
        }

        public int search_index_of_vertex(string name) // находит индекс вершины в списке EdgeList по ее имени
        {
            
            if (Vertexes.Count == 0)
                return -1;
            for(int i = 0; i < Vertexes.Count; i++)
            {
                if (Vertexes[i].Name == name)
                    return i;
            }
            return -1; // возвращает нереальное для индекса значение в случае отсутствия искомой вершины
        }

        bool check_necessary_condition() // проверка на выполнение необходимого условия существования гамильтонова цикла
        { // если вершина А есть в Vertexes, то должна найтись другая вершина, из которой можно прийти в А
            if (is_empty())
                return false;
            foreach (VertexWithEdges v in Vertexes)
            {
                if (v.Edges.Count == 0)
                    return false;
                foreach (EdgeEnd edge in v.Edges)
                {
                    if (search_index_of_vertex(edge.Name) < 0)
                        return false; // это значит, что нашлась вершина, В которую есть ребро, но ИЗ которой нет
                }
            }
            return true;
        }

        uint count_edges()
        {
            uint amount_of_edges = 0;
            if (Vertexes.Count() > 0)
            {
                foreach (VertexWithEdges v in Vertexes)
                {
                    if (v.Edges.Count() > 0)
                    {
                        foreach (EdgeEnd edge in v.Edges)
                            amount_of_edges++;
                    }
                }
            }
            return amount_of_edges;
        }

        int choose_edge(int vertex_index, ref double sum_distance, double alpha = 1, double betta = 1) // параметры функции выбора
        {
            if (Vertexes[vertex_index].Edges.Count > 0)
            {
                List<int> indexes = new List<int>(); // индексы не посещенных вершин
                double sum_of_all_probabilities = 0; // знаменатель формулы
                foreach (EdgeEnd edge in Vertexes[vertex_index].Edges)
                {
                    int now_index = search_index_of_vertex(edge.Name);
                    if (!Vertexes[now_index].IsVisited)
                    {
                        indexes.Add(now_index);
                        sum_of_all_probabilities += Math.Pow(edge.Feromon, alpha) * Math.Pow(1/ edge.Distance, betta);
                    }
                }
                if (indexes.Count > 0) // если есть непосещенные вершины, в которые можно пойти
                {
                    if (indexes.Count == 1)
                    {
                        sum_distance += Vertexes[vertex_index].Edges[Vertexes[vertex_index].has_edge_to(Vertexes[indexes[0]].Name)].Distance;
                        Vertexes[indexes[0]].IsVisited = true; // помечаем вершину как посещенную
                        return indexes[0]; // если есть лишь один подходящий вариант, то гадать не нужно
                    }
                    double[] probabilities = new double[indexes.Count];
                    for (int i = 0; i < indexes.Count; i++)
                    { // рассчитывается функция распределения верятности для каждой вершины
                        probabilities[i] = Math.Pow(Vertexes[vertex_index].Edges[Vertexes[vertex_index].has_edge_to(Vertexes[indexes[i]].Name)].Feromon, alpha) * Math.Pow(1 / Vertexes[vertex_index].Edges[Vertexes[vertex_index].has_edge_to(Vertexes[indexes[i]].Name)].Distance, betta) / sum_of_all_probabilities;
                        if (i > 0)
                            probabilities[i] += probabilities[i - 1]; // каждая вероятность - отрезок, где левый конец >= 0 или верхней границы предыдущей вероятности, а правый <=1 или зависит от длины промежутка
                    }
                    double probability = random.NextDouble(); // число от 0 до 1 попадет в промежуток вероятности какой-то вершины
                    if (probability < probabilities[0])
                    {
                        sum_distance += Vertexes[vertex_index].Edges[Vertexes[vertex_index].has_edge_to(Vertexes[indexes[0]].Name)].Distance;
                        Vertexes[indexes[0]].IsVisited = true; // помечаем вершину как посещенную
                        return indexes[0]; // возврат индекса выбранной вершины
                    }
                    for (int i = 1; i < indexes.Count; i++)
                    {
                        if (probability >= probabilities[i - 1] && probability < probabilities[i])
                        {
                            sum_distance += Vertexes[vertex_index].Edges[Vertexes[vertex_index].has_edge_to(Vertexes[indexes[i]].Name)].Distance;
                            Vertexes[indexes[i]].IsVisited = true; // помечаем вершину как посещенную
                            return indexes[i]; // возврат индекса выбранной вершины
                        }
                    }
                }
            }
            return -1; // в случае, если некуда идти, индекс возвращается невозможным
        }

    public bool ant_algorithm(uint steps = 10, double alpha = 1, double betta = 1) // параметры для функции выбора пути
        {
            
            if (check_necessary_condition())
            {
                uint step_number = 0; // номер итерации (порядковый номер муравья, пытающегося найти путь)
                uint changed_times = 0;
                int start_vertex_index = 0;
                List<int> path = new List<int>(); // сохраняет гамильтонов путь (хранит индексы вершин): первая и последняя вершина одинаковые
                double prev_path_length = 0;
                while (true) // цикл по всем муравьям
                {
                    clear_marks(); // очищаем пометки
                    path.Clear();
                    step_number++;
                    start_vertex_index = random.Next(0, Vertexes.Count() - 1);
                    Vertexes[start_vertex_index].IsVisited = true;
                    int iterations = Vertexes.Count; // количество ребер в гамильтоновом цикле = числу вершин графа
                    int next_index = start_vertex_index; // индекс вершины
                    double path_length = 0;
                    path.Add(next_index);
                    while (iterations > 0) // цикл поиска пути 
                    {
                        iterations--;
                        next_index = choose_edge(next_index, ref path_length);
                        path.Add(next_index);
                        if (next_index < 0)
                            break;
                        if (iterations == 1)
                        {
                            int end = Vertexes[next_index].has_edge_to(Vertexes[start_vertex_index].Name); // индекс ребра, ведущего из предпоследней вершины гамильтонова цикла в последнюю (она же первая)
                            if (end >= 0)
                            { //тогда нашелся гамильтонов путь
                                path.Add(start_vertex_index);
                                path_length += Vertexes[next_index].Edges[end].Distance; // добавляем к длине пути длину последнего ребра
                                double feromon_amount = 1 / path_length; 
                                for (int i = 0; i < path.Count - 1; i++) // увеличивает феромон вдоль пути
                                    Vertexes[path[i]].Edges[Vertexes[path[i]].has_edge_to(Vertexes[path[i + 1]].Name)].Feromon += feromon_amount;
                                if (path_length > prev_path_length) // значит, было улучшение
                                    changed_times++; 
                                prev_path_length = path_length; // запоминаем длину пути для будущего сравнения
                                iterations--;
                            }
                        }

                    }
                    if (step_number % steps == 0) // критерий выхода: если в течение 10 шагов ни разу не было улучшения
                    {
                        if (changed_times == 0)
                        {
                            if (path.Count == (Vertexes.Count + 1) && path[0] == path[path.Count - 1])
                            { // проверяется, что найден нужный путь - у него верная длина, и конец и начало - одно и то же
                                Console.WriteLine("Найденный гамильтонов цикл (суммарная длина = {0}):", path_length);
                                foreach (int i in path)
                                {
                                    Console.Write("{0} ", Vertexes[i].Name);
                                }
                                return true;
                            }
                            else
                                Console.WriteLine("Гамльтонов цикл не найден");
                            return false;
                        }
                        else
                            changed_times = 0;
                    }
                }

            }
            Console.WriteLine("Гамльтонов цикл не найден.");
            return false;
        }

    }
}
