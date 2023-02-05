using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{

    public class Sorter
    {
        class Individual
        {
            /// <summary>
            /// 适应度，此处为路径的倒数，越大越好
            /// </summary>
            public float Fitness
            {
                get
                {
                    return fitness < 0 ? GetFitness() : fitness;
                }
            }

            public List<int> Gene
            {
                get => gene;
                set
                {
                    gene = value;
                    fitness = -1;
                }
            }

            private List<int> gene;
            private float fitness;
            public Individual(List<int> gene = null)
            {
                if (gene != null)
                    this.Gene = gene;
                else
                    SetDefaultGene();
            }

            public float GetFitness()
            {
                float distance = 0;
                for (int i = 0; i < gene.Count - 1; i++)
                {
                    distance += SoterHelper.DistanceMatrix[i, i + 1];
                }
                fitness = 1f / distance;
                return fitness;
            }

            public void SetDefaultGene()
            {
                gene = new List<int>();
                for (int i = 0; i < SoterHelper.DistanceMatrix.GetLength(0); i++)
                {
                    gene.Add(i);
                }
                gene.Shuffle();
            }

            public Individual Clone()
            {
                return new Individual(this.Gene);
            }
        }
        //float[,] distanceMatrix;

        public void InitMatrix(List<Vector3> points)
        {
            SoterHelper.DistanceMatrix = new float[points.Count, points.Count];
            for (int i = 0; i < SoterHelper.DistanceMatrix.Length; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    SoterHelper.DistanceMatrix[i, j] = points[i].GetDisdance(points[j]);
                    SoterHelper.DistanceMatrix[j, i] = SoterHelper.DistanceMatrix[i, j];
                }
                SoterHelper.DistanceMatrix[i, i] = 0;
            }
        }

        public List<int> Solve(int population = 50, int iterTimes = 300)
        {
            //遗传算法
            //每一代最优的个体
            List<Individual> bestList = new List<Individual>();

            List<Individual> individualList = new List<Individual>();//个体列表
            for (int i = 0; i < population; i++)
            {
                individualList.Add(new Individual());
            }
            for (int i = 0; i < iterTimes; i++)
            {
                //交叉
                Cross(individualList, out List<Individual> nextGen);
                //变异
                Mutate(nextGen);
                //选择
                Select(individualList, nextGen, population, out Individual best);
                bestList.Add(best);
            }

            //选出最优
            Individual bestIndi = bestList[0];
            foreach (var item in bestList)
            {
                if (item.Fitness < bestIndi.Fitness) bestIndi = item;
            }
            return bestIndi.Gene;
        }

        private void Cross(List<Individual> individualList, out List<Individual> nextGen)
        {
            if (individualList.Count % 2 == 1) individualList.Add(new Individual());
            nextGen = new List<Individual>();
            for (int i = 0; i < individualList.Count; i += 2)
            {
                var fatherGene = individualList[i].Gene;
                var motherGene = individualList[i + 1].Gene;
                //子代
                List<int> childGene0 = new List<int>();
                List<int> childGene1 = new List<int>();
                for (int j = 0; j < fatherGene.Count; j++)
                {
                    childGene0[j] = fatherGene[j];
                    childGene1[j] = motherGene[j];
                }
                //交换的基因片段的位置
                int randomIndexHead = SoterHelper.RandomRange(0, fatherGene.Count - 1);
                int randomIndexTail = SoterHelper.RandomRange(randomIndexHead, fatherGene.Count);
                //交换基因
                for (int j = 0; j < fatherGene.Count; j++)
                {
                    if (j < randomIndexHead || j > randomIndexTail) continue;
                    //开始“交换”基因
                    //单纯的交换父母基因会导致孩子基因有重复元素。所以孩子0的基因是父亲基因调换了一些元素位置的版本。
                    //child0基因：
                    int index;
                    index = fatherGene.FindIndex((item) => item == motherGene[j]);
                    if (index != -1)
                        (childGene0[index], childGene0[j]) = (childGene0[j], childGene0[index]);

                    //child1基因
                    index = motherGene.FindIndex((item) => item == fatherGene[j]);
                    if (index != -1)
                        (childGene1[index], childGene1[j]) = (childGene1[j], childGene1[index]);
                }
                nextGen.Add(new Individual(childGene0));
                nextGen.Add(new Individual(childGene1));
            }
        }

        /// <summary>
        /// 每个个体有<paramref name="prob"/>的概率变异，变异个体中每位基因有<paramref name="geneMutateProb"/>的概率变异
        /// </summary>
        /// <param name="individualList"></param>
        /// <param name="prob"></param>
        /// <param name="geneMutateProb"></param>
        /// <param name="keepOriginal">是否保留变异前的副本</param>
        private void Mutate(List<Individual> individualList, float prob = 0.2f, float geneMutateProb = 0.1f, bool keepOriginal = false)
        {
            int forCount = individualList.Count;
            for (int i = 0; i < forCount; i++)
            {
                if (SoterHelper.Random01() > prob) continue;
                if (keepOriginal) individualList.Add(individualList[i].Clone());
                var gene = individualList[i].Gene;
                for (int j = 0; j < gene.Count; j++)
                {
                    if (SoterHelper.Random01() > geneMutateProb) continue;
                    int randomIndex0 = SoterHelper.RandomRange(0, gene.Count - 1);
                    int randomIndex1 = SoterHelper.RandomRange(randomIndex0, gene.Count);
                    (gene[randomIndex0], gene[randomIndex1]) = (gene[randomIndex1], gene[randomIndex0]);
                }
            }
        }

        private void Select(List<Individual> individualList, List<Individual> nextGen, int population, out Individual best)
        {
            individualList.Concat(nextGen);
            individualList.Sort((x, y) =>
            {
                return x.Fitness.CompareTo(y.Fitness);
            });
            if (individualList.Count > population)
                individualList.RemoveRange(population - 1, individualList.Count - 1);
            best = individualList[0];
        }
    }

    public static class SoterHelper
    {
        private static float[,] distanceMatrix;

        public static float[,] DistanceMatrix { get => distanceMatrix; set => distanceMatrix = value; }

        private static Random random = new Random();

        public static void Shuffle(this IList list)
        {
            object temp;
            int tempIndex;
            for (int i = 0; i < random.Next(0, 100); i++)
            {
                tempIndex = random.Next(0, list.Count);
                temp = list[tempIndex];
                list[tempIndex] = list[i];
                list[i] = temp;
            }
        }

        public static int RandomRange(int min, int max) => random.Next(min, max);
        public static float Random01() => (float)random.NextDouble();
    }
}
