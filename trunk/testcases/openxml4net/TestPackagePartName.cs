/* ====================================================================
   Licensed to the Apache Software Foundation (ASF) under one or more
   contributor license agreements.  See the NOTICE file distributed with
   this work for Additional information regarding copyright ownership.
   The ASF licenses this file to You under the Apache License, Version 2.0
   (the "License"); you may not use this file except in compliance with
   the License.  You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
==================================================================== */

using NPOI.OpenXml4Net.OPC;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace TestCases.OPC
{
    [TestClass]
    public class TestPackagePartName
    {

        /**
         * Test method GetExtension().
         */
        [TestMethod]
        public void TestGetExtension()
        {
            PackagePartName name1 = PackagingUriHelper.CreatePartName("/doc/props/document.xml");
            PackagePartName name2 = PackagingUriHelper.CreatePartName("/root/document");
            Assert.AreEqual("xml", name1.Extension);
            Assert.AreEqual("", name2.Extension);
        }
    }

}


