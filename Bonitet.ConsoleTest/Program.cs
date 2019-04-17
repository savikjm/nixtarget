using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Printing;
using System.IO;
using TuesPechkin;
using System.Drawing;
using System.Web;
using System.Security.Cryptography.X509Certificates;
using Bonitet.ConsoleTest.ServiceReference1;
using System.Net;
using System.Xml;
using Bonitet.DAL;
using System.Data.OleDb;
using System.Data;
using System.Data.Linq;
using System.Linq;
using Newtonsoft.Json;
using Bonitet.Document;

namespace Bonitet.ConsoleTest
{
    class Program
    {

        static void Main(string[] args)
        {
            //var str = "1x3byZfNsQs5zJhDGXSR 5AMi1Gpye5FdAjdBMWlm GV2lWjB0cJmw3Wcpk9h9 jFJyedgPUHDhiE2sL3pH H6PXZjQr3CCZbVamSbhc iBR2aOV3seI8tQJdilYq X4yczOGcvvgt3bsvNUEq 4V4rDgElYCJS4PudWq2m 1REzJUvHdxKurwJ5kKRm BmiIFxFhYHA0ksAqrTDJ oTLwiepsESfNGSusWNuz Oy2EYNo7nZr8FmBpLeHg LDkELxuSfHxAPWdpEmR8 EvrcnwoiUrqEKuEfsvfz 5BFafBGWFXiEuVzgnLsx FuxDwk0gadcZTIPCcxnA uStQWr8TLI4I2o9zMTW6 y6VIiKv4lpiKcKLf5QET OKJqBDNee0FBs8khLFqq K8aNITWtleN3dDZ3I6h5 P9tHVJB6eJJFiSxrWL2O LKcIo4fDcTD0o1snISEg mBZcDNJDYs15YMM1wa8C au6IlUWI8EihxxntNRI7 gO5D6QPti05V0uZgzi3K kmSvP8YO4IutxJutXSTq 5WfxiUBoq2HqNpHaFmx7 wlNp0wNSrGUaJ8vslpR1 jg0vxFxyL0xFNdCh8ySy sZo7Y7ZrFxLvzKlTcs8L auoNKkDWr7IrIHWQW2KQ zW7XptwJLnhzwNI0Gn1x bnTGYvlv5EANVxyxIdfn J5Ssex0eRj12rZnaVJbM amsyjyOlz8CcsHoL7IZW LCVTxCDvjoZPJ46XLoQ1 nHajylZ4eIIfI9COKGjR VwOWe9Xhra2OYZJRzAmn KtZLY0ttoqtUE6Apjw53 gNdvmyyeloRvCTFRkwLl HQRAMARnUB2g02I34nLL 9cdehAyyIYMABTX8lv82 hIsiXzTvfdRUq1Y1UsSH 7O9lmGquUV5wAK5oREZW WlhjomqNdLUoe1fPg49x qK7MZVCClrnIkiAFuMpA x7ACQszAkoZpgecBhPuU ggdCBt9bRxlC5vD2Leey kAAQFjhEMs40DDdK1DI1 TIGK7B6mi02791D6GD9w solH6m7HKD9Y3jikluZn znPyfkdAgnYN83XWoQoa oINDd26WiPPKSoCtGDQh cpPy2thWJG43QosR1Z07 eUCkAMUj9DmfQSuKqv0w ehJ1gLKwyvL8ox3pRTRf lhjw1eex8s2slNb6cU0Z vV5FUIO5ZTVToM3xZZoj yEpSqnhVlLgnJvvT2dVh Mtfc7WbLgaFt3jq5uSDF 5DJLcoKEhz8nbtw6UMJB lSMpEKIaS3bnSxHEg86Q DMubkV4rx75ycr6QF56y DIK54i39ZVlHktoqym5H KKUTnHisYzeeoQUd7VHE 4gI6SpaS5Y8nqO76wh86 Fz3Tr9ptEvtekzv2bpY9 iMBOlp6FVxWfVn8GveP0 YBzFZnAz5aRkbKEU8IRC EX3UNXTPQ3qecWKbsjyu QnGVkahS5r5FQpc5c2Mg e74TTdwczx2pf5INksf9 wzOYGCd85f3swjRzbZdo gbs8NO6mXnClazLdwPxA u28ZWcaYcaVVm3DhPIt7 TCMzp6Q896tL8ouklTwG WUzH2yU9F0qELyvF9FQV ZCqCjc0RyyMko6GW3Lfj DQFI7ndWH42a4EbVSSUf JiL0u0YKp1vJP0QAQtuL zUwjFV8Vo39fVw5MlMuX 1Zj7PMs7OPFVAElTneio 3m58ENpTAHyFhfRSQ4t7 xpQy3TyRAN0BOiErPwcY AsSs1Dtgz4SZoFuogcd2 Jttk9ovk5y8rJNt0fNnd t9ud3RKggkd4hEJutHcG 7M29hWx7HiXPXJP1ZU61 skxR9BjKq4syZ7L3EWxA CvmMQstM3nxUdTx5CIzs TNwPbaGbR6tAt7iUswgR j4HqKAjZuSv7cxqehrmS rfUYpJailDh6GbFKJLQ6 ekpGuPXSYh1lcda8Y976 O7yUU3QsQv6mCmxlblxA drPRGgmt10oW67UaXpGB MOftvRVwvyY3Sqg0zT5r Y6PfAAahNCaYjfT308KE waKHi7NpaqpJG2hpwqoD Pkpl8xGKuFwg12APFaLr f1z9fRGnNyR4kbNAFkjg 72oS6Qpd6Yjsw6XHfO7K nAzkqo6gFHxnUNRjzohJ revEJJniUP5dRWn4RuaA CCB1Njai3YvhgeW5k90B kpXLlrnt0Wro0LY28Bcb M34EtfwHnn6VcZppjjBo n1Ham4GrBlBiVKx2eJ7X uCcTF3tXySRxbbQqB8Fx tZgfuyJAJNfemkRb3laZ XiPuTAw2Kq6lLAXwnahk bYVMWr0u3FAo5js70NqN 28oc2gvDJULJrOkxW2gR Pkh3dlMbcggwlmQhlHQD Ms9ktbpdxRZY0gAqwSzt IEy6N6Fovas6Tjqp3diF ungTFyReE3WuPpk2HXhM SLTcsy9Dy4XTMG2edNhS 0lgrQQCIPNlTQ9YAaWa1 pGAahOuX7seE6Q8onMqQ Hqh35UtHjBgo0JIAdHTD mDfCX9ptfzocnJKKHvw6 PzJcMYuzTxSfrbi7ia7f NTUQvytrtT7PWX0PRpGE quGRvtnvIdbbKIQvdE0n zMrMNWrcmoRSc6uiPOST IdxFQdrEjiuZBrcDwQmy 3D9LjZEiiVy67j6LxKSi Rl7TNMszMHMGB7l13yOb HRpB67S5nyKjGO8iAdQI XGrNEB61Y9ZFB3gybwaI 62z8Obl5DUB43CNUS0U9 QlnE3gOxaRGode26dsfw FtE85fWfKs535B8tZB3w ht9F594bPslLX4UNLznw L4pGp95H6cJPSlAS0U4G GXsZ347glCofNvue9I9h 4KnADXBRlv1W1tvBdrG9 hd65ANLEVP6rFZhzkGsG i8dRi53rA087l799paxd 7dmA15tK64NerCMtZdJL BZX4DvCOtDrhsc4jqjy2 Xvf7ymrgNTaKBTLQABu7 z0poTCd4XoUAfp05xIPW j8snySDxudKpnwTLYQ0z hSRtFRKHenAOpuckcWxW pAPIKihJgoKwCGIjemRE DZY8sVFjUlbG80jjdaoH ZexFxT2sgY6ioaYb7pfg JfYdqwJrmDv9RbjA4cIM I6kgMkmrTYgHJV4rNfac NaSTyb1bog80qiC9xoO5 GnNsX0IqL6PEJ3413iUV 5KrMKNVBzTKRjM0LvU1i W9CZ55DoVBkO7m41Zgib Wm0wxndkwNXDMR1EXTFd j34ABtFYfJDuUm1gv9w6 Cj6AOj8fpZJOZW8KStIF tXqCIgjdR6bVifDmzmA9 TbU7PRtdFPDdMp7ApTeB 4Abz2QKahny1LAIxIwa9 lHRkLOTAzFdBPfwvFIoa FqNrcSJU8lnLVlr1bR9w EmqUjdB2HkxcdgvKHdT3 tkCgOeq7MsXr85h1YTsE exhIjRB5rVNpxDRE8vHM Xqpzad3NcHOZ6r6EgJx0 5oS6tmVVwc7HhrwRRg2q cSnwjrfsyWGojqzDZ384 8q80HXfXqMghLb7DAaNX fvSFCgNCJ4ALgCiCjXZy 4BLTMwW8e9hTYK0hUaK1 t8zcgYjb0rBECaI7SQUw jnM3aOYa3zMFRpkUS3mv J9g3Zr4Ip65Kuiy7MM0O ZaLgb0XZdar7z8uKoURE 75MR7AOxNefOYXDtRvOB kBMFr3ntraRHGLwXETAo WPnzFWNiIGKQQlenFPN9 RIpY3Ye0PyAceLk1MFK1 jo6jF6wzPbJhHeW66olH btJrSWqPRsPZ2jphMH15 UBSCNOCXPzwfgLZVLIHs PAYHa4agVwP9Bz5o6UTN Nk41G3u7P2z0RyljGghl QOM98qnhLa7XgWd0uTCZ 83Ret4nkxZUmlbJIvVV2 2svTi37khNfayvBvZdpo p9wRUZwuoSMeGtZZJ7s6 XzL0QVnanodQASmho1uE qC6qBlVRCVkASz0uyfo3 V6kmJwgrd7Un9fAr72sf Ia2YzzrXMaAXk7C0BtNP jPEV8AmChk88I3rBV6kq fXYhrmaqfHlF6ALYFdRM fBmEdfdfMdpJfdFE1NTc ZUUtsoO0CESwN2XMcurJ BG7zuoh1ViBhuJRBS0HQ 0mHZUfxhCQAu22jGvf6g juEmSoBiOmsT1dnmL4aM v8TDe6FrM3X0nDrNg3Wi QnDTpqmYUVuh6cm6bvMj DKoMrtv0r8CDoSIGomLc jZiKLNWkD7H7Wefr8Uc6 ufg3p8NYuWdoEbWRfXlJ jSoOGWddl2IvNISTJMKW 65PI2TmYsLDwp0te0ZJU kLm2KoeEqLvcMPvkPT5M 95yBhw09SzR311ESMBPq 8uhem4WAbfu32TnmJwsy RX9n60YdvSNqSTNRo1SA 4uPy0kuRTtSd0rm1Dtmh GXzoMaZufzq6iuG9zPkF QCT3kSboMqgCfBZ8hTYm w2s4SReDra6lvDoczJji 1mqEVTH1Ujj8xoOfNEPR WSxktuLjhYIkmC6jU4S3 UDkVYAxB6lBDCQDyEstE lSw6GLZbN8ELnKsvC9ti b99fGwYWpK05dbZmO97d HE1DBFnLFBoxlUDLq9CK knI2qevFICmpYiZStm3r uMXRLAYuB8LaeALO1nfZ s0mnuxds7MKWtW0QQxbg vzkqdXK6I1I5xtB21091 UreQay1PxRfjyhL4d7sX 1f5L5cjqZSkqV4gzfb10 hWroGfG1IMOZU1DAdS5h PCseau1mJqPRPCftSrKG CqLbgAToBcktjHWEnVfq 3h6EfEXIr5xi9FjXbVB1 VyaVNFMWQuz00zNPykPM hx2vmYTGYHjZtfCIgsli 85D2SB1dUB3EigrihMOS GfV68hYcE8v5BIM0eXaQ eWP89iiN3HSMPbrpNYO5 e8P26Ta7fG0cNJUItBFm yl9potLCVHh76GGszgid cvU2NIJ0dpAsctEax4cy TbgCxtCipRWKNCQT6SR4 9i1Io19qq0y9JScfCT4U yEY5qQ9BH57p4opPC5im lkOT9vEkjhGHi8TTFZuN 9W6ej1UlhRP1C5cdDBaZ EEaAZzbqOE4fFhDtKHU4 mtvTr42lb89Cr02BVgbB KAI6OGaPCyOGwhnFnnZ8 MKKxTwBUluse2lYvYWUK CgdtBJZZxDuF2LRLcErL fpOF9cajGr6CW8nEmWud aHJJtthVPitYUApyOg9U Ls8yjYAcoUQYARmYa63B zZEUS5jea9EPlz7fClsb IkWCDoSg4JVQfwdjtJR5 YFG5872ztQFOwhfdeSbX hPEzW8e4qPAus6znPvyl AeQdaqi6o7CPzMgTNakw IvthxDNlKeIJDV05FgjA CXb1sVgFpcVurbXQBvtr 4fHT2S2a30urUviVCKYH QvALbwq5XM67LBptn1cd GyJkJjBsh3enTTbEWvqn vUPqXygQSp8NJUc9PODN zJt5BoLw15nvFbfWeVIH SPgdUp6JCfIYY60anrnj 8UrSY4rRKk8Ce4RlJ2c0 dlF6lv6TT9drPEKgwHIX OTP4HLMqcA1pzjxrMn0j vSE2KhhDWGWIWJjjQ5BQ ESnXR20H5dJfaAfRh1bE m2GZ3qFoXxtkOcuUaVl9 zTNzQENroZRXNJT8W9Y8 koXv4Y7zMTeZ5tnUYSzC dxOUCnw5F92XXqpebEUU hw2Oic6zRnLm9gm495hI tgUu2D6fQ0zDmqpQTQDY T6PT8NezRhc4zhCDSxWZ LnJjkinENcOZFZUERtYR i5NsuULPylm0Acx1eyQw jJDEz7NyzpeFb6fl566U oSorj0riB2VSoDPuSgwU D6a4WnUgia29LObQzGXR RYyKuA7AxgeRmbe8qtjQ wzCeZpLFyvIIfJmvFsGR rZCArKHU76HIrG8flYVp vjrpUo58hZNL3xa33ifF gX5p2VotfHbxCy1gZJjF C2locoo05nnaZVmi18Xk Gw1TVXfCHhlOZ8HlqsMh 4LwnzzxTtzTM4nkSD7wj uMwX0dU7PYmDuL1y4WHv MmQEINhVFkpiIgFNKI6n Wry9tTBzpYe2wGkiix4u fofytTUdaDxaOggXQZtJ XQaUYNvtg1gqDKa1YIWN 4YMfzwLT3hhwd0lc84Hd PvEVrWL6DVoRKZZSejys UqxpGNZqfFdHP3Ekmvs7 DjSYvupeaj9fOmCEoarK RMGbui91htDr17eQRDAV eauCjPMxjvjS8Kusuzkj VbfPhcnAvqrIF5H9JDvT VJgZZxNZ2HS1BrviHMYS ShlzbZLBl36xjvz0riR5 iuB8BzvDqz600xzw9U7x NsJfmV4ZBHDJMvw2pstk VHOZtyTJmJNDB6cuaaRb EZ6bKrgsoT6MNjFzbK3S bmRRMChB3Xr96aQdNTiL gny1L85YwhDJ5glQ54Q3 qTiZ3hGjaxXPrYCexqxt c1ZCIzOo8JmLhz4KiAbp wDPol9OSESyPeu83eKbo Rr0I4LSeIBKFzFxy7B5C pxbvheEoOL9n9Z32gRJV xBG44WYOIuJ0Iym5L8PQ ylR49QltrnzsQSz2QNEv wxbq6eevgYdZwZZ0F2jw EV55cWoGe148CkrkXqvh 21C1REwNN7XUWqME4Wo8 JX8ZCjXQIeSsw6GeXMpk wMu58k7ZV8BumE6AjP73 rmW8HXrr1IAGxUBLSIZh 8dT0tnXrzS0RFWy3zTqu VYiceVkxVEH8J0kq92jc FzKa8cRX2TdGa3GrClq6 qwEt5eNcv4B436SdgDsO RBdCwlmd1sJrsdv7Z51H GTNJYExyfHdJXObzdfUO mDRSyQJ44NrmozYsc2cc 3sB9AXe1axSfaJRxYm44 SHl7TOzFxc9WFhi4F4lF vujd0nJVVtxUm11O37LC 1Bz0CmklSJF9tNBR14Bj hdQuOuTvsuTPQRWz7Tk9 kXvIW3YSeLjdzy5W50I5 E8skM6FLVxjXhwsyxypG 29wrOxV6bZLimZ5OfBVg 91ov5WKyWJ0Ldl5FEnm1 sxucQ4C0CfOSdVjBMHNm d5XyiHofjPn0KJqndpou 2pcLvF6pNXmkR4uiHmYU nhZHCeGWP9MVyd4cBqNv 4C563NRhJhC40yY5erDx 35WSfEWDzopl58k8ll8o XEcL2dcKZwRctfo1SZ9B iOpnNGs0Erxd1HCfZ8DL 7OBjrfSAZgNWOiKSmPJD aWveIJynwxkLQ8cRq5aV lggh1NgANMdEs68kkBYS DwczOLQtNWPvKpqQ4fRp tmdeG7Mmi74OFO4eGO6R Dc3sLqnjSqmaf8a9HfXZ EtUlSykTUskUwwtmO0Ga FJxkdK7mG8xSIwFISyzL tk22SAxFq8f0UZMZVWas W9ypC2nPxe6O6o5UiVv3 jt9EpuPHmcwj9cYsYzRQ uaSIrSV7fwmisji5GtZr Lk4nqJe0qJz6znATF7QD XTG1l0PTctnxpbKXUK1e FHmK5rOMgiMhVvVsCNZh ualY7ZaKS9uiapKVVf8e TiPREXgFhb6L1lRP0fHV bZGmJKZwOMtTtr1Tdkn9 M33TeSqozBO2ERizXFD4 vpkBhkAZpOwe7cSxY6ol 23kSvHwbtVQpjhbXA4Qh fIfyzzN60pFtYA9xCZeT 39j7Vp7SA6QaEhKcQRrx hrFZEFJwOnL4Z16r9NwQ wCpRrhpsXvX9PAaPPr5n XCrSp6OYIBPHyPJ6dB60 aMO5HXZ3JXu1kubfncP9 AiNyctw78SM61b1yCCFY OmtR4ZfrKXEUejyO6Fe7 qZA9vOXziMLmJTn7wf1B 9tRbPxGTz17hUvuBND67 En4N78P6cWiRZfcoDB2i VSNsQy0JmfK9DoT2yC2z QxYqar5uSFy41R8eEp5e n40wdSrEXI9m489QutxW A3b75LwTleqXaRGWRDcQ 0QH0WRjMHTkPwFaD1Bud VndeLNKuWYHPvFffxfaH OtqpURS0dmhmfaLmjKKX mfGDYFvvIudu46drk0k1 TkNncM1Zh1k9LCH3bHpC dAfYsprOuKFPQUAF8itK T2wG6lKjcjW4FobUpJVs h44eYVtATISWUal0hfIy QiPypd0tLnCDLtEYaQdS 5MBc5KNBpr9ZK8WOF9Pl MRxSFRJoWyOpDqqr5O08 FTUOHkdyWJIfKvBO0PP2 v9fWQRkxfS5UBIrMPFgj D5V2KuxNVW3mZ7G6QPYG zH0E88e3VTCxJ3z2fsrm zluhoOKbsi1n8eQXxN14 t8OfZ01cBJO2WTSOLq5x SaLTTDmKvelOvADJSIjI aR3ulKM90bYZQONcAV0L AN1rtM9Ko2WE6pZ1V4Vg q3uPVfEs8nicv5wfqOp8 vKMhRnQoUfvRT1ae2FzB KLER6aoLxslLdfEvk4RI 0TlpSDvLlvPRkQuWBkeK djyFjcDJWKikvXSt5gCJ Vl4O2ZsE2TRWTpjV5MSZ GtN8Ov1jpg9cecb7Le1I M3T3IDEcqwM830YIOsrW 1Crg9x7P9aYe0mHID757 pI4RsKbTOqjZmGS9Pg1p nhQghL24bEtkvqAqPqAq xDPrJT2oEaFHLiqg0fDx fHllyzBHL6riRA8Gft15 yKtZ5Fl67oeauxY6fGUR 8iIvJ3OnzWdqc7PwFvOp QrQZ194dfDtELomCalCL 2hkTGBKn7yFSzbcFdT25 VWOo8ygQDIggCKlga0sO e3OQH2uPIZne8pqm2WAt XvAbAzCwe4aj6qQkL0U0 gMQORdzVp5fy9bNWyARB qkDqyKBI8wFh3tNdKfZT avwY4anYvcEfOAscmK8I pGVKmDkUbkfkQIMM3qWH GWFzNpRMmnHF232ksiRK fNMMHkg6jy0g8xdvXA47 6QXcD4Tyk2ozlSkGQ2rI V2875BXCz0MtNxj2L4kK dicbnfwsNXIehguxjprT EYxLxQg1X9Nfpap1oamQ h8FbmF0CFyd92DteL88w 8KXgWoOZ42NJkh3t5Kf4 FGDt859gDjN9rh8MPCti tGzDML9g2TE2BLHMVvJ0 13pJUUwdcVa0NePDp1Q6 Ai5sI0jgmZDWmNNs3R23 5vgRAMApL96uGR1qYFIw 9OIRMTVMd5chTlBwJpzt BISztCF5FMbo28mYuxpL P0GTf1uNBbN66FubvWJy lfiMcWlZtOOVJZ6LJuAw JFC1xVhONCR6pnUYeQwg eAZZubqyHOzncFZqMohH KfaQJVbzgdBGEhBPvvc0 y56z0MJJ6Q8w46v8hw1h XKSFhHK4vVciBa1yDG2V 49rcADWe1ywd4570fP3z EAloQLYg8lyPcmcLMmHs 4VETCccUtwP9k4J8AdI5 3drCdWEiBMRSVI0pPH8L SFB6yyHZKtaEKJ0oySMh X2B3ZmmnctbDZPDizQRf KbRPkuoXrBZ3dH1vIUpN WW2rsWqGWbUS2jAU6dk7 UK0Myw3VFrF0LAyzXIeg x2FHQfnBSAso6g3jp5a5 cfW1FQrZzIgFiyIdeoko 0iZEJQne6FFjXIyQqdx8 hGdKFtifnbJTvmhmIIVR tmnRxni0cOi5tY762QHA XnsimoJQiDK0HiA131P2 7sdXdQ5OsJlKE6pkmopk gbDJPBdaI8lhalWe3KTp 4Q5rqRvxdy68agvsXee3 jKI20DjV34OzFvRlUZjZ zN17Hf8Sv9JcslkZZauh r1fZtj4jsASUC5QWomqt camJaTZ1SdGK2AjEYJ3K w0PXTZtm0U9Ueq4efoJp SnKQd5aqLvg10pNudpCO kEOX4eSxLmwkfqYuHktj UQYSXuUA45kMduGJQEko tpvnFh0Uv92ZgmMN6PEk Mwz";
            //str = str + str + str + str + str + str + str + str + str + str;

            //Bonitet.DAL.DALHelper.GetXmlResuls(22620);
            //Bonitet.DAL.DALHelper.InsertCrmResponse("6591574", 1, str);
            //Bonitet.Document.DocumentClass.SetReport1Data(1, "6228151", 2014, "");
            //GetCRMBlokadaNaSmetka();
            //GetCRMKompletenIzvestaj();

            //GetDataFromFile();

            // var res = CRM_DocumentClass.SaveCRM_Account("4057465", 2014, false);
        }
        public static void GetDataFromFile()
        {
            string line;

            // Read the file and display it line by line.
            System.IO.StreamReader file1 =
               new System.IO.StreamReader("c:\\nix\\edb.txt");

            System.IO.StreamWriter file2 =
             new System.IO.StreamWriter("c:\\nix\\no_embs.txt", true);

            System.IO.StreamWriter file3 =
             new System.IO.StreamWriter("c:\\nix\\not_in_db.txt", true);


            var lineCount = File.ReadLines("c:\\nix\\edb.txt").Count();

            while ((line = file1.ReadLine()) != null)
            {
                var tmp = line.Split(';');

                var embs = tmp[0];
                var value = tmp[1];

                if (embs == "NULL" || embs == "0")
                {
                    file2.WriteLine(line);
                    continue;
                }

                var res = DALHelper.InsertEDBToCompany(embs, value);

                if (res == false)
                    file3.WriteLine(line);

                lineCount--;
                Console.WriteLine(lineCount);
            }

            file1.Close();
            file2.Close();
        }

        public static void GetCRMKompletenIzvestaj()
        {
            var tmpRes = new List<Dictionary<string, object>>();
            //var db = new DAL.BiznisMreza.DALDataContext();

            //var tmp = db.Subjekts.Skip(100).Take(60).ToList();

            //foreach (var item in tmp)
            //{

            //4328337 - jaca 
            /*
             5171105 - Субјектот нема доставено годишна сметка или известување дека нема деловна активност во 2014 година.
             5224195 - Субјектот нема доставено годишна сметка или известување дека нема деловна активност во 2014 година.
             5237955 - Субјектот нема доставено годишна сметка или известување дека нема деловна активност во 2014 година.
             5491495 - Субјектот нема доставено годишна сметка или известување дека нема деловна активност во 2014 година.
             5498465 - Субјектот нема доставено годишна сметка или известување дека нема деловна активност во 2014 година.
             5890705 - Субјектот нема доставено годишна сметка или известување дека нема деловна активност во 2014 година.
             5953545 - Субјектот нема доставено годишна сметка или известување дека нема деловна активност во 2014 година.
             5493005 - Субјектот нема доставено годишна сметка или известување дека нема деловна активност во 2014 година.
             5507855 - Субјектот нема доставено годишна сметка или известување дека нема деловна активност во 2014 година.
             5501288 - Субјектот нема доставено годишна сметка или известување дека нема деловна активност во 2014 година.
             */
            var EMBS = "6311792";
            var res = Bonitet.CRM.CRM_ServiceHelper.GetCRM_Account(EMBS, 2014);

            XmlDocument doc = new XmlDocument();

            doc.LoadXml(res);

            var CompanyValues = new Dictionary<int, double>();
            var CompanyDetails = new Company();

            foreach (XmlElement data in doc.SelectNodes("CrmResponse"))
            {
                var i = 0;
                var tmpDic = new Dictionary<string, object>();

                foreach (XmlElement child in data.SelectNodes("CrmResultItems"))
                {
                    if (i == 0)
                    {
                        CompanyDetails.Name = ExtractString(child.InnerXml, "LEFullName");
                        CompanyDetails.Mesto = ExtractString(child.InnerXml, "Place");
                        CompanyDetails.EMBS = ExtractString(child.InnerXml, "LEID");
                        i++;

                        tmpDic.Add(EMBS, CompanyDetails);
                        continue;
                    }

                    foreach (XmlElement child1 in child.SelectNodes("CrmResultItem"))
                    {
                        var AccountNo = ExtractString(child1.InnerXml, "AccountNo");
                        var CurrentYear = ExtractString(child1.InnerXml, "CurrentYear");

                        double value = 0.0;

                        double.TryParse(CurrentYear, out value);


                        CompanyValues.Add(Int32.Parse(AccountNo), value);

                    }

                    tmpDic.Add(EMBS, CompanyValues);
                }
                tmpDic.Add(EMBS, "CRM No Response");

                tmpRes.Add(tmpDic);
            }
            //    }
            //}

            var jsonRes = JsonConvert.SerializeObject(tmpRes);

            return;



            //var strRes = "AccountNo\tAccountName\tPrevious\tBruto\tChange\tCurrentYear";

            //XmlDocument doc = new XmlDocument();

            //doc.LoadXml(res);

            //foreach (XmlElement data in doc.SelectNodes("CrmResponse"))
            //{
            //    var i = 0;
            //    foreach (XmlElement child in data.SelectNodes("CrmResultItems"))
            //    {
            //        foreach (XmlElement child1 in child.SelectNodes("CrmResultItem"))
            //        {
            //            var AccountNo = ExtractString(child1.InnerXml, "AccountNo");
            //            var AccountName = ExtractString(child1.InnerXml, "AccountName");
            //            var Previous = ExtractString(child1.InnerXml, "Previous");
            //            var Bruto = ExtractString(child1.InnerXml, "Bruto");
            //            var Change = ExtractString(child1.InnerXml, "Change");
            //            var CurrentYear = ExtractString(child1.InnerXml, "CurrentYear");

            //            double value = 0.0;

            //            double.TryParse(CurrentYear, out value);

            //            strRes += AccountNo + "\t" + AccountName + "\t\"" + Previous + "\"\t\"" + Bruto + "\"\t\"" + Change + "\"\t\"" + CurrentYear + "\"\n";
            //            //CompanyValues.Add(Int32.Parse(AccountNo), value);

            //        }

            //    }

            //}
        }

        public static void PopulateDBForKompletenIzvestaj()
        {
            var EMBS = "4001192";
            var Year = 2014;
            var compID = 0;
            var res = Bonitet.CRM.CRM_ServiceHelper.GetCRM_Account(EMBS, Year);

            //var res = DALHelper.GetXmlResuls(compID);

            XmlDocument doc = new XmlDocument();

            doc.LoadXml(res);

            var CompanyValues = new Dictionary<int, double>();
            var CompanyDetails = new Company();

            foreach (XmlElement data in doc.SelectNodes("CrmResponse"))
            {
                var i = 0;
                foreach (XmlElement child in data.SelectNodes("CrmResultItems"))
                {
                    if (i == 0)
                    {
                        CompanyDetails.Name = ExtractString(child.InnerXml, "LEFullName");
                        CompanyDetails.Mesto = ExtractString(child.InnerXml, "Place");
                        CompanyDetails.EMBS = ExtractString(child.InnerXml, "LEID");
                        i++;
                        continue;
                    }

                    foreach (XmlElement child1 in child.SelectNodes("CrmResultItem"))
                    {
                        var AccountNo = ExtractString(child1.InnerXml, "AccountNo");
                        var CurrentYear = ExtractString(child1.InnerXml, "CurrentYear");

                        double value = 0.0;

                        double.TryParse(CurrentYear, out value);


                        CompanyValues.Add(Int32.Parse(AccountNo), value);

                    }

                }

            }

            Bonitet.DAL.DALHelper.InsertCompanyValues(CompanyDetails, CompanyValues, EMBS, Year, res);
        }

        public static void GetCRMBlokadaNaSmetka()
        {
            var res = CRM.CRM_ServiceHelper.GetCRM_AccountStatus("06788912");
            //var EMBS = ExtractString(res, "LEID");
            //var AccInfo = ExtractString(res, "AccInfo");
            //var TimeStamp = ExtractString(res, "TimeStamp");
            //var InfoMessage = ExtractString(res, "InfoMessage");

            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;

            //X509Store store = new X509Store(StoreName.My);
            //store.Open(OpenFlags.ReadOnly);
            //var cert = store.Certificates.Find(X509FindType.FindBySubjectName,"E=info@targetgroup.mk, CN=UserTargetGroup, OU=for test purposes only, O=Target Group, L=Skopje, S=Makedonija, C=MK",true);

            //ServiceReference1.XmlWebServiceSoapClient ws = new ServiceReference1.XmlWebServiceSoapClient();
            //ws.ClientCredentials.ClientCertificate.Certificate = store.Certificates[0];


            //var strParameters = "<?xml version=\"1.0\" encoding=\"utf-8\"?><CrmRequest ProductName=\"LEInfoBlockedbyBankAccountsTARGET\"><Parameters TemplateName=\"LEInfoBlockedBA\"><Parameter Name=\"@LEID\">{0}</Parameter></Parameters></CrmRequest>";

            //string strResult = string.Empty;

            //List<string[]> results = new List<string[]>();

            //XmlSoapHeader header = new XmlSoapHeader();

            //var tmp = string.Format(strParameters, "5133394");
            //strResult = ws.ProcessRequest(ref header, tmp);


            return;

        }


        public static string ExtractString(string s, string tag)
        {
            // You should check for errors in real-world code, omitted for brevity
            try
            {
                var startTag = "<" + tag + ">";
                int startIndex = s.IndexOf(startTag) + startTag.Length;
                int endIndex = s.IndexOf("</" + tag + ">", startIndex);
                return s.Substring(startIndex, endIndex - startIndex);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
