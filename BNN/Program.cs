using System;
using System.Linq;
using System.Numerics;
using NumSharp;

namespace BNN
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            double[][] sample_in =
            {
                new double[] {4.21850347, 2.23419161}, new double[] {0.90779887, 0.45984362},
                new double[] {-0.27652528, 5.08127768}, new double[] {0.08848433, 2.32299086},
                new double[] {3.24329731, 1.21460627}, new double[] {1.44193252, 2.76754364},
                new double[] {1.0220286, 4.11660348}, new double[] {3.97820955, 2.37817845},
                new double[] {0.58894326, 4.00148458}, new double[] {1.25185786, 0.20811388},
                new double[] {0.62835793, 4.4601363}, new double[] {1.68608568, 0.65828448},
                new double[] {1.18454506, 5.28042636}, new double[] {0.06897171, 4.35573272},
                new double[] {1.78726415, 1.70012006}, new double[] {4.4384123, 1.84214315},
                new double[] {3.18190344, -0.18226785}, new double[] {0.30380963, 3.94423417},
                new double[] {0.73936011, 0.43607906}, new double[] {1.28535145, 1.43691285},
                new double[] {1.1312175, 4.68194985}, new double[] {0.66471755, 4.35995267},
                new double[] {1.31570453, 2.44067826}, new double[] {-0.18887976, 5.20461381},
                new double[] {2.57854418, 0.72611733}, new double[] {0.87305123, 4.71438583},
                new double[] {1.3105127, 0.07122512}, new double[] {0.9867701, 6.08965782},
                new double[] {1.42013331, 4.63746165}, new double[] {2.3535057, 2.22404956},
                new double[] {2.43169305, -0.20173713}, new double[] {1.0427873, 4.60625923},
                new double[] {0.95088418, 0.94982874}, new double[] {2.45127423, -0.19539785},
                new double[] {1.62011397, 2.74692739}, new double[] {2.15504965, 4.12386249},
                new double[] {1.38093486, 0.92949422}, new double[] {1.98702592, 2.61100638},
                new double[] {2.11567076, 3.06896151}, new double[] {0.56400993, 1.33705536},
                new double[] {-0.07228289, 2.88376939}, new double[] {2.50904929, 5.7731461},
                new double[] {-0.73000011, 6.25456272}, new double[] {1.37861172, 3.61897724},
                new double[] {0.88214412, 2.84128485}, new double[] {2.22194102, 1.5326951},
                new double[] {2.0159847, -0.27042984}, new double[] {1.70127361, -0.47728763},
                new double[] {-0.65392827, 4.76656958}, new double[] {0.57309313, 5.5262324},
                new double[] {1.956815, 0.23418537}, new double[] {0.76241061, 1.16471453},
                new double[] {2.46452227, 6.1996765}, new double[] {1.33263648, 5.0103605},
                new double[] {3.2460247, 2.84942165}, new double[] {1.10318217, 4.70577669},
                new double[] {2.85942078, 2.95602827}, new double[] {1.59973502, 0.91514282},
                new double[] {2.97612635, 1.21639131}, new double[] {2.68049897, -0.704394},
                new double[] {1.41942144, 1.57409695}, new double[] {1.9263585, 4.15243012},
                new double[] {-0.09448254, 5.35823905}, new double[] {2.72756228, 1.3051255},
                new double[] {1.12031365, 5.75806083}, new double[] {1.55723507, 2.82719571},
                new double[] {0.10547293, 3.72493766}, new double[] {2.84382807, 3.32650945},
                new double[] {3.15492712, 1.55292739}, new double[] {1.84070628, 3.56162231},
                new double[] {1.28933778, 3.44969159}, new double[] {1.64164854, 0.15020885},
                new double[] {3.92282648, 1.80370832}, new double[] {1.70536064, 4.43277024},
                new double[] {0.1631238, 2.57750473}, new double[] {0.34194798, 3.94104616},
                new double[] {1.02102468, 1.57925818}, new double[] {2.66934689, 1.81987033},
                new double[] {0.4666179, 3.86571303}, new double[] {0.94808785, 4.7321192},
                new double[] {1.19404184, 2.80772861}, new double[] {1.15369622, 3.90200639},
                new double[] {-0.29421492, 5.27318404}, new double[] {1.7373078, 4.42546234},
                new double[] {0.46546494, 3.12315514}, new double[] {0.08080352, 4.69068983},
                new double[] {3.00251949, 0.74265357}, new double[] {2.20656076, 5.50616718},
                new double[] {1.36069966, 0.74802912}, new double[] {2.63185834, 0.6893649},
                new double[] {2.82705807, 1.72116781}, new double[] {2.91209813, 0.24663807},
                new double[] {1.1424453, 2.01467995}, new double[] {1.05505217, -0.64710744},
                new double[] {2.47034915, 4.09862906}, new double[] {-1.57671974, 4.95740592},
                new double[] {1.41164912, -1.32573949}, new double[] {3.00468833, 0.9852149},
                new double[] {-0.63762777, 4.09104705}, new double[] {0.829832, 1.74202664}
            };

            double[][] sample_ex =
            {
                new double[] {1}, new double[] {1}, 
                new double[] {0}, new double[] {0}, 
                new double[] {1}, new double[] {0}, 
                new double[] {0}, new double[] {1}, 
                new double[] {0}, new double[] {1},
                new double[] {0}, new double[] {1}, 
                new double[] {0}, new double[] {0}, 
                new double[] {1}, new double[] {1}, 
                new double[] {1}, new double[] {0}, 
                new double[] {1}, new double[] {1},
                new double[] {0}, new double[] {0},
                new double[] {1}, new double[] {0}, 
                new double[] {1}, new double[] {0}, 
                new double[] {1}, new double[] {0}, 
                new double[] {0}, new double[] {1}, 
                new double[] {1}, new double[] {0},
                new double[] {1}, new double[] {1}, 
                new double[] {1}, new double[] {0}, 
                new double[] {1}, new double[] {1}, 
                new double[] {0}, new double[] {1},
                new double[] {0}, new double[] {0},
                new double[] {0}, new double[] {0},
                new double[] {1}, new double[] {1}, 
                new double[] {1}, new double[] {1}, 
                new double[] {0}, new double[] {0}, 
                new double[] {1}, new double[] {1}, 
                new double[] {0}, new double[] {0},
                new double[] {0}, new double[] {0}, 
                new double[] {0}, new double[] {1}, 
                new double[] {1}, new double[] {1}, 
                new double[] {1}, new double[] {0}, 
                new double[] {0}, new double[] {1},
                new double[] {0}, new double[] {1},
                new double[] {0}, new double[] {0}, 
                new double[] {1}, new double[] {0}, 
                new double[] {0}, new double[] {1}, 
                new double[] {1}, new double[] {0},
                new double[] {0}, new double[] {0},
                new double[] {1}, new double[] {1}, 
                new double[] {0}, new double[] {0}, 
                new double[] {1}, new double[] {0}, 
                new double[] {0}, new double[] {0}, 
                new double[] {0}, new double[] {0},
                new double[] {1}, new double[] {0},
                new double[] {1}, new double[] {1}, 
                new double[] {1}, new double[] {1}, 
                new double[] {1}, new double[] {1}, 
                new double[] {0}, new double[] {0}, 
                new double[] {1}, new double[] {1},
                new double[] {0}, new double[] {1}
            };

            Matrix data_in = new Matrix(sample_in);
            Matrix data_ex = new Matrix(sample_ex);
            
            test();

            Network network = new Network(data_in, data_ex, 1);

            network.train(100);

            //Predict(data_in, data_ex, network.Neurons[0]);
            /*Console.WriteLine(network.Neurons[0].Weights.ToString());
            Console.WriteLine(network.Neurons[0].Bias);*/
        }

        public static bool Predict(Matrix X, Matrix data_ex, neuron neuron)
        {
            var A = neuron.model(X, neuron.Weights, neuron.Bias);

            return A[0][0] >= 0.5;
        }

        public static void test()
        {
            var test = np.random.randn(100, 1);

            var res = test.ToJaggedArray<double>();
            
        }
    }
}