namespace Noise
{
    public static class FirFilter
    {
        public const int SampleFilterTapNum = 256;

        private static double[] _filterTaps = 
            new double[] 
            {
                0.0000015683623995207874,
  0.0000012842258633780393,
  4.5100946913308713e-7,
  -0.000002585530739959959,
  -0.000009369371899941103,
  -0.00002178320096271681,
  -0.000041870023551301247,
  -0.00007155004234549936,
  -0.00011224479289127277,
  -0.0001644467668807776,
  -0.00022729276477909376,
  -0.00029821810041984166,
  -0.00037277481615640687,
  -0.0004446933183951952,
  -0.0005062441887684609,
  -0.0005489215428260187,
  -0.0005644185731075004,
  -0.0005458104938822859,
  -0.0004888059752374285,
  -0.00039288660488893934,
  -0.0002621329944764186,
  -0.00010554566966488812,
  0.00006328836297438234,
  0.0002272556384393454,
  0.00036747287272026713,
  0.00046559495227089766,
  0.0005064165396142936,
  0.0004804189817443997,
  0.00038586668930980597,
  0.00023006615076874828,
  0.000029471842894282376,
  -0.0001915435645644931,
  -0.00040325338574072247,
  -0.0005742641523356509,
  -0.0006759109100322997,
  -0.0006867787321405413,
  -0.0005966709546574122,
  -0.00040934398852381164,
  -0.00014343882814548076,
  0.00016874901593318654,
  0.00048472364423455367,
  0.0007571852583787232,
  0.0009407470069771195,
  0.0009990743661032,
  0.0009113788111163643,
  0.0006771801030028579,
  0.0003184077228595338,
  -0.00012176416171885832,
  -0.0005835096604621183,
  -0.0009979970739297927,
  -0.0012971970570658043,
  -0.0014245264952500844,
  -0.0013447356128488375,
  -0.001051402859379681,
  -0.0005706305524880207,
  0.0000400148866896984,
  0.0006975938398135458,
  0.0013044362909227622,
  0.0017620727941927007,
  0.0019865633302564517,
  0.0019229255083679773,
  0.001556304357195004,
  0.0009178240097849121,
  0.00008372215370248892,
  -0.0008326901156124081,
  -0.0016956017095261554,
  -0.0023660762744978953,
  -0.002723581400947788,
  -0.0026864562300135775,
  -0.002227994905435936,
  -0.0013852350995631202,
  -0.00025844943916197963,
  0.0009993569691349499,
  0.0022021170383478704,
  0.0031571970072803188,
  0.00369510173918465,
  0.003697972019661706,
  0.003122288052011589,
  0.0020117359207108125,
  0.000497417360384296,
  -0.0012156502721436002,
  -0.0028747240625027043,
  -0.004215121477549197,
  -0.005000757727952899,
  -0.005063461933964099,
  -0.004334820385684346,
  -0.002864973647089799,
  -0.0008243947285189048,
  0.0015129832378392,
  0.003804144885058012,
  0.0056849218852641735,
  0.006825110334957418,
  0.00698280587198381,
  0.006049485341030126,
  0.004078131601841046,
  0.0012887023313757544,
  -0.0019517281119274166,
  -0.0051731660618877,
  -0.007866464333335816,
  -0.009558549986518715,
  -0.009888635642793565,
  -0.008673989001836276,
  -0.005954474669524695,
  -0.002007457622721689,
  0.002671689723311553,
  0.007423450610687392,
  0.011507444147694574,
  0.014205475607904949,
  0.014931163853147928,
  0.01333106861744405,
  0.009362343170019057,
  0.0033341807516750225,
  -0.0040954924303308665,
  -0.011971031567532904,
  -0.01913442232617237,
  -0.024355375996487596,
  -0.02648135339654215,
  -0.024589231418474876,
  -0.01811914856288854,
  -0.006972435047803657,
  0.008440636308982915,
  0.02721168113876662,
  0.04800485704875839,
  0.06918103275099856,
  0.08896222382607373,
  0.10561764148940082,
  0.11764958116254749,
  0.12395699077776451,
  0.12395699077776451,
  0.11764958116254749,
  0.10561764148940082,
  0.08896222382607373,
  0.06918103275099856,
  0.04800485704875839,
  0.02721168113876662,
  0.008440636308982915,
  -0.006972435047803657,
  -0.01811914856288854,
  -0.024589231418474876,
  -0.02648135339654215,
  -0.024355375996487596,
  -0.01913442232617237,
  -0.011971031567532904,
  -0.0040954924303308665,
  0.0033341807516750225,
  0.009362343170019057,
  0.01333106861744405,
  0.014931163853147928,
  0.014205475607904949,
  0.011507444147694574,
  0.007423450610687392,
  0.002671689723311553,
  -0.002007457622721689,
  -0.005954474669524695,
  -0.008673989001836276,
  -0.009888635642793565,
  -0.009558549986518715,
  -0.007866464333335816,
  -0.0051731660618877,
  -0.0019517281119274166,
  0.0012887023313757544,
  0.004078131601841046,
  0.006049485341030126,
  0.00698280587198381,
  0.006825110334957418,
  0.0056849218852641735,
  0.003804144885058012,
  0.0015129832378392,
  -0.0008243947285189048,
  -0.002864973647089799,
  -0.004334820385684346,
  -0.005063461933964099,
  -0.005000757727952899,
  -0.004215121477549197,
  -0.0028747240625027043,
  -0.0012156502721436002,
  0.000497417360384296,
  0.0020117359207108125,
  0.003122288052011589,
  0.003697972019661706,
  0.00369510173918465,
  0.0031571970072803188,
  0.0022021170383478704,
  0.0009993569691349499,
  -0.00025844943916197963,
  -0.0013852350995631202,
  -0.002227994905435936,
  -0.0026864562300135775,
  -0.002723581400947788,
  -0.0023660762744978953,
  -0.0016956017095261554,
  -0.0008326901156124081,
  0.00008372215370248892,
  0.0009178240097849121,
  0.001556304357195004,
  0.0019229255083679773,
  0.0019865633302564517,
  0.0017620727941927007,
  0.0013044362909227622,
  0.0006975938398135458,
  0.0000400148866896984,
  -0.0005706305524880207,
  -0.001051402859379681,
  -0.0013447356128488375,
  -0.0014245264952500844,
  -0.0012971970570658043,
  -0.0009979970739297927,
  -0.0005835096604621183,
  -0.00012176416171885832,
  0.0003184077228595338,
  0.0006771801030028579,
  0.0009113788111163643,
  0.0009990743661032,
  0.0009407470069771195,
  0.0007571852583787232,
  0.00048472364423455367,
  0.00016874901593318654,
  -0.00014343882814548076,
  -0.00040934398852381164,
  -0.0005966709546574122,
  -0.0006867787321405413,
  -0.0006759109100322997,
  -0.0005742641523356509,
  -0.00040325338574072247,
  -0.0001915435645644931,
  0.000029471842894282376,
  0.00023006615076874828,
  0.00038586668930980597,
  0.0004804189817443997,
  0.0005064165396142936,
  0.00046559495227089766,
  0.00036747287272026713,
  0.0002272556384393454,
  0.00006328836297438234,
  -0.00010554566966488812,
  -0.0002621329944764186,
  -0.00039288660488893934,
  -0.0004888059752374285,
  -0.0005458104938822859,
  -0.0005644185731075004,
  -0.0005489215428260187,
  -0.0005062441887684609,
  -0.0004446933183951952,
  -0.00037277481615640687,
  -0.00029821810041984166,
  -0.00022729276477909376,
  -0.0001644467668807776,
  -0.00011224479289127277,
  -0.00007155004234549936,
  -0.000041870023551301247,
  -0.00002178320096271681,
  -0.000009369371899941103,
  -0.000002585530739959959,
  4.5100946913308713e-7,
  0.0000012842258633780393,
  0.0000015683623995207874




            };

        public static void Init(SampleFilter sampleFilter)
        {

        }

        public static void Put(SampleFilter sampleFilter, double input)
        {
            sampleFilter.History[sampleFilter.LastIndex++] = input;

            if(sampleFilter.LastIndex == SampleFilterTapNum)
                sampleFilter.LastIndex = 0;
        }

        public static double Get(SampleFilter sampleFilter)
        {
            double acc = 0;
            int index = (int)sampleFilter.LastIndex;

            for(int i = 0 ; i < SampleFilterTapNum ; i++)
            {
                index = index != 0 ? index-1 : SampleFilterTapNum - 1;
                acc += sampleFilter.History[index] * _filterTaps[i];
            }

            return acc;
        }
    }

    public class SampleFilter
    {
        public SampleFilter()
        {
            History = new double[FirFilter.SampleFilterTapNum];
        }

        public double[] History { get; private set; }
        public uint LastIndex { get; set; }
    }
}