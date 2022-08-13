using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PathFind
{
    public static class PathFind
    {
        public static Path<Node> FindPath<Node>(
            Node start,
            Node destination,
            Func<Node, Node, double> distance,
            Func<Node, double> estimate)
            where Node : IHasNeighbours<Node> //제약조건 where 절
            //IHasNeighbours -> bool값으로 canpass 를 던져줘서 사용을 하면 다른곳에서도 언제든지 이동불가 타일을 변경이 가능
        {
            var closed = new HashSet<Node>();
            var queue = new PriorityQueue<double, Path<Node>>();
            queue.Enqueue(0, new Path<Node>(start));

            while (!queue.IsEmpty)
            {
                var path = queue.Dequeue();

                if (closed.Contains(path.LastStep))
                    continue;
                if (path.LastStep.Equals(destination))
                    return path;

                closed.Add(path.LastStep);

                //제약조건을 검으로써 접근이 가능하게 됨
                foreach (Node n in path.LastStep.Neighbours)
                {
                    double d = distance(path.LastStep, n);
                    var newPath = path.AddStep(n, d);
                    queue.Enqueue(newPath.TotalCost + estimate(n), newPath);
                }
                // 제약조건 struct -> 값으로만 접근이 가능하게 됨
            }

            return null;
        }
    }
}