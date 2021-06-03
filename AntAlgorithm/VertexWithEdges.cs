using System;
using System.Collections.Generic;
using System.Text;

namespace AntAlgorithm
{
    class VertexWithEdges
    { // хранит вершину, а также список ребер, исходящих из нее
        public string Name { get; } // название вершины

        public List<EdgeEnd> Edges { get; protected set; } // список исходящих из вершины ребер
        public bool IsVisited { get; set; } // пометки, была ли посещена данная вершина

        protected VertexWithEdges()
        {
            Edges = new List<EdgeEnd>();
        }
        public VertexWithEdges(string name)
        {
            Name = name;
            Edges = new List<EdgeEnd>();
        }

        public int has_edge_to(string name) // возвращает индекс ребра из начальной в вершину с именем name
        {
            for (int i = 0; i < Edges.Count; i++)
            {
                if (Edges[i].Name == name)
                    return i;
            }
            return -1;
        }

        public double sum_distance()
        {
            double sum = 0;
            foreach (EdgeEnd edge in Edges)
                sum += edge.Distance;
            return sum;
        }
    }
}
