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

namespace TestCases.HPSF.Basic
{
    using System;
    using System.IO;
    using System.Text;
    using System.Collections;
    using NUnit.Framework;
    using NPOI.HPSF;
    using NPOI.HPSF.Wellknown;
    using NPOI.Util;
    using NPOI.POIFS.FileSystem;

    /**
     * Basing on: src/examples/src/org/apache/poi/hpsf/examples/ModifyDocumentSummaryInformation.java
     * This class Tests Reading and writing of meta data. No actual document is Created. All information
     * is stored in a virtal document in a MemoryStream
     * @author Matthias Güter
     * @since 2006-03-03
     * @version $Id: TestEmptyProperties.java 353563 2004-06-22 16:16:33Z klute $
     */
    [TestFixture]
    public class TestMetaDataIPI
    {

        private MemoryStream bout = null;//our store
        private POIFSFileSystem poifs = null;
        private DirectoryEntry dir = null;
        DocumentSummaryInformation dsi = null;
        SummaryInformation si = null;

        [SetUp]
        public void SetUp()
        {
            bout = new MemoryStream();
            poifs = new POIFSFileSystem();
            dir = poifs.Root;
            dsi = null;
            try
            {
                DocumentEntry dsiEntry = (DocumentEntry)
                    dir.GetEntry(DocumentSummaryInformation.DEFAULT_STREAM_NAME);
                DocumentInputStream dis = new DocumentInputStream(dsiEntry);
                PropertySet ps = new PropertySet(dis);
                dis.Close();
                dsi = new DocumentSummaryInformation(ps);


            }
            catch (FileNotFoundException)
            {
                /* There is no document summary information yet. We have to Create a
                 * new one. */
                dsi = PropertySetFactory.CreateDocumentSummaryInformation();
                Assert.IsNotNull(dsi);
            }
            catch (IOException)
            {
                ////e.printStackTrace();
                Assert.Fail();
            }
            catch (NoPropertySetStreamException)
            {
                ////e.printStackTrace();
                Assert.Fail();
            }
            catch (MarkUnsupportedException)
            {
                ////e.printStackTrace();
                Assert.Fail();
            }
            catch (UnexpectedPropertySetTypeException)
            {
                ////e.printStackTrace();
                Assert.Fail();
            }
            Assert.IsNotNull(dsi);
            try
            {
                DocumentEntry dsiEntry = (DocumentEntry)
                    dir.GetEntry(SummaryInformation.DEFAULT_STREAM_NAME);
                DocumentInputStream dis = new DocumentInputStream(dsiEntry);
                PropertySet ps = new PropertySet(dis);
                dis.Close();
                si = new SummaryInformation(ps);


            }
            catch (FileNotFoundException)
            {
                /* There is no document summary information yet. We have to Create a
                 * new one. */
                si = PropertySetFactory.CreateSummaryInformation();
                Assert.IsNotNull(si);
            }
            catch (IOException)
            {
                ////e.printStackTrace();
                Assert.Fail();
            }
            catch (NoPropertySetStreamException)
            {
                ////e.printStackTrace();
                Assert.Fail();
            }
            catch (MarkUnsupportedException)
            {
                ////e.printStackTrace();
                Assert.Fail();
            }
            catch (UnexpectedPropertySetTypeException)
            {
                ////e.printStackTrace();
                Assert.Fail();
            }
            Assert.IsNotNull(dsi);
        }

        /**
         * Setting a lot of things to null.
         */
        private void TearDown()
        {
            bout = null;
            poifs = null;
            dir = null;
            dsi = null;

        }


        /**
         * Closes the MemoryStream and Reads it into a MemoryStream.
         * When finished writing information this method is used in the Tests to
         * start Reading from the Created document and then the see if the results match.
         *
         */
        private void CloseAndReOpen()
        {

            try
            {
                dsi.Write(dir, DocumentSummaryInformation.DEFAULT_STREAM_NAME);
                si.Write(dir, SummaryInformation.DEFAULT_STREAM_NAME);
            }
            catch (WritingNotSupportedException)
            {
                ////e.printStackTrace();
                Assert.Fail();
            }
            catch (IOException)
            {
                ////e.printStackTrace();
                Assert.Fail();
            }

            si = null;
            dsi = null;
            try
            {

                poifs.WriteFileSystem(bout);
                bout.Flush();

            }
            catch (IOException)
            {

                ////e.printStackTrace();
                Assert.Fail();
            }

            Stream is1 = new MemoryStream(bout.ToArray());
            Assert.IsNotNull(is1);
            poifs = null;
            try
            {
                poifs = new POIFSFileSystem(is1);
            }
            catch (IOException)
            {

                ////e.printStackTrace();
                Assert.Fail();
            }
            try
            {
                is1.Close();
            }
            catch (IOException)
            {
                ////e.printStackTrace();
                Assert.Fail();
            }
            Assert.IsNotNull(poifs);
            /* Read the document summary information. */
            dir = poifs.Root;

            try
            {
                DocumentEntry dsiEntry = (DocumentEntry)
                    dir.GetEntry(DocumentSummaryInformation.DEFAULT_STREAM_NAME);
                DocumentInputStream dis = new DocumentInputStream(dsiEntry);
                PropertySet ps = new PropertySet(dis);
                dis.Close();
                dsi = new DocumentSummaryInformation(ps);
            }
            catch (FileNotFoundException ex)
            {
                Assert.Fail(ex.Message);
            }
            catch (IOException e)
            {
                //e.printStackTrace();
                Assert.Fail(e.Message);
            }
            catch (NoPropertySetStreamException e)
            {
                //e.printStackTrace();
                Assert.Fail(e.Message);
            }
            catch (MarkUnsupportedException e)
            {
                //e.printStackTrace();
                Assert.Fail(e.Message);
            }
            catch (UnexpectedPropertySetTypeException e)
            {
                //e.printStackTrace();
                Assert.Fail(e.Message);
            }
            try
            {
                DocumentEntry dsiEntry = (DocumentEntry)
                    dir.GetEntry(SummaryInformation.DEFAULT_STREAM_NAME);
                DocumentInputStream dis = new DocumentInputStream(dsiEntry);
                PropertySet ps = new PropertySet(dis);
                dis.Close();
                si = new SummaryInformation(ps);


            }
            catch (FileNotFoundException)
            {
                /* There is no document summary information yet. We have to Create a
                 * new one. */
                si = PropertySetFactory.CreateSummaryInformation();
                Assert.IsNotNull(si);
            }
            catch (IOException)
            {
                //e.printStackTrace();
                Assert.Fail();
            }
            catch (NoPropertySetStreamException)
            {
                //e.printStackTrace();
                Assert.Fail();
            }
            catch (MarkUnsupportedException)
            {
                //e.printStackTrace();
                Assert.Fail();
            }
            catch (UnexpectedPropertySetTypeException)
            {
                //e.printStackTrace();
                Assert.Fail();
            }
        }

        /**
         * Sets the most important information in DocumentSummaryInformation and Summary Information and reReads it
         *
         */
        [Test]
        public void TestOne()
        {

            //DocumentSummaryInformation
            dsi.Company = "xxxCompanyxxx";
            dsi.Manager = "xxxManagerxxx";
            dsi.Category = "xxxCategoryxxx";

            //SummaryInformation
            si.Title = "xxxTitlexxx";
            si.Author = "xxxAuthorxxx";
            si.Comments = "xxxCommentsxxx";
            si.Keywords = "xxxKeyWordsxxx";
            si.Subject = "xxxSubjectxxx";

            //Custom Properties (in DocumentSummaryInformation
            CustomProperties customProperties = dsi.CustomProperties;
            if (customProperties == null)
            {
                customProperties = new CustomProperties();
            }

            /* Insert some custom properties into the container. */
            customProperties.Put("Key1","Value1");
            customProperties.Put("Schlüsel2", "Wert2");
            customProperties.Put("Sample Integer", 12345);
            customProperties.Put("Sample Boolean", true);
            DateTime date = new DateTime(1988,1,5,5,34,12);
            customProperties.Put("Sample Date", date);
            customProperties.Put("Sample Double", -1.0001);
            customProperties.Put("Sample Negative Integer", -100000);

            dsi.CustomProperties = customProperties;

            //start Reading
            CloseAndReOpen();

            //Testing
            Assert.IsNotNull(dsi);
            Assert.IsNotNull(si);

            Assert.AreEqual("xxxCategoryxxx", dsi.Category, "Category");
            Assert.AreEqual("xxxCompanyxxx", dsi.Company, "Company");
            Assert.AreEqual("xxxManagerxxx", dsi.Manager, "Manager");

            Assert.AreEqual( "xxxAuthorxxx", si.Author);
            Assert.AreEqual("xxxTitlexxx", si.Title);
            Assert.AreEqual("xxxCommentsxxx", si.Comments);
            Assert.AreEqual("xxxKeyWordsxxx", si.Keywords);
            Assert.AreEqual("xxxSubjectxxx", si.Subject);


            /* Read the custom properties. If there are no custom properties yet,
             * the application has to Create a new CustomProperties object. It will
             * serve as a container for custom properties. */
            customProperties = dsi.CustomProperties;
            if (customProperties == null)
            {
                Assert.Fail();
            }

            /* Insert some custom properties into the container. */
            String a1 = (String)customProperties["Key1"];
            Assert.AreEqual("Value1", a1, "Key1");
            String a2 = (String)customProperties["Schlüsel2"];
            Assert.AreEqual("Wert2", a2, "Schlüsel2");
            int a3 = (int)customProperties["Sample Integer"];
            Assert.AreEqual(12345, a3, "Sample Number");
            Boolean a4 = (Boolean)customProperties["Sample Boolean"];
            Assert.AreEqual(true, a4, "Sample Boolean");
            DateTime a5 = (DateTime)customProperties["Sample Date"];
            Assert.AreEqual(date, a5, "Custom Date:");

            Double a6 = (Double)customProperties["Sample Double"];
            Assert.AreEqual(-1.0001, a6, "Custom Float");

            int a7 = (int)customProperties["Sample Negative Integer"];
            Assert.AreEqual(-100000, a7, "Neg");
        }


        /**
         * multiplies a string
         * @param s Input String
         * @return  the multiplied String
         */
        public String Elongate(String s)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 10000; i++)
            {
                sb.Append(s);
                sb.Append(" ");
            }
            return sb.ToString();
        }



        /**
         * Test very long input in each of the fields (approx 30-60KB each)
         *
         */
        [Test]
        public void TestTwo()
        {

            String company = Elongate("company");
            String manager = Elongate("manager");
            String category = Elongate("category");
            String title = Elongate("title");
            String author = Elongate("author");
            String comments = Elongate("comments");
            String keywords = Elongate("keywords");
            String subject = Elongate("subject");
            String p1 = Elongate("p1");
            String p2 = Elongate("p2");
            String k1 = Elongate("k1");
            String k2 = Elongate("k2");

            dsi.Company = company;
            dsi.Manager = manager;
            dsi.Category = category;

            si.Title = title;
            si.Author = author;
            si.Comments = comments;
            si.Keywords = keywords;
            si.Subject = subject;
            CustomProperties customProperties = dsi.CustomProperties;
            if (customProperties == null)
            {
                customProperties = new CustomProperties();
            }

            /* Insert some custom properties into the container. */
            customProperties.Put(k1, p1);
            customProperties.Put(k2, p2);
            customProperties.Put("Sample Number", 12345);
            customProperties.Put("Sample Boolean", true);
            DateTime date = new DateTime(1984, 5, 16, 8, 23, 15);
            customProperties.Put("Sample Date", date);

            dsi.CustomProperties = customProperties;


            CloseAndReOpen();

            Assert.IsNotNull(dsi);
            Assert.IsNotNull(si);
            /* Change the category to "POI example". Any former category value will
             * be lost. If there has been no category yet, it will be Created. */
            Assert.AreEqual(category, dsi.Category, "Category");
            Assert.AreEqual(company, dsi.Company, "Company");
            Assert.AreEqual(manager, dsi.Manager, "Manager");

            Assert.AreEqual(author, si.Author);
            Assert.AreEqual(title, si.Title);
            Assert.AreEqual(comments, si.Comments);
            Assert.AreEqual(keywords, si.Keywords);
            Assert.AreEqual(subject, si.Subject);


            /* Read the custom properties. If there are no custom properties
             * yet, the application has to Create a new CustomProperties object.
             * It will serve as a container for custom properties. */
            customProperties = dsi.CustomProperties;
            if (customProperties == null)
            {
                Assert.Fail("customProperties cannot be null");
            }

            /* Insert some custom properties into the container. */
            String a1 = (String)customProperties[k1];
            Assert.AreEqual(p1, a1, "Key1");
            String a2 = (String)customProperties[k2];
            Assert.AreEqual(p2, a2, "Schlüsel2");
            int a3 = (int)customProperties["Sample Number"];
            Assert.AreEqual(12345, a3, "Sample Number");
            Boolean a4 = (Boolean)customProperties["Sample Boolean"];
            Assert.AreEqual(true, a4, "Sample Boolean");
            DateTime a5 = (DateTime)customProperties["Sample Date"];
            Assert.AreEqual(date, a5, "Custom Date:");


        }


        /**
         * Adds strange characters to the string
         * @param s Input String
         * @return  the multiplied String
         */
        public String Strangize(String s)
        {
            StringBuilder sb = new StringBuilder();
            String[] umlaute = { "ä", "ü", "ö", "Ü", "$", "Ö", "Ü", "É", "Ö", "@", "ç", "&" };
            int j = 0;
            Random rand = new Random();
            for (int i = 0; i < 5; i++)
            {
                sb.Append(s);
                sb.Append(" ");
                j = rand.Next(220);
                j += 33;
                Console.WriteLine(j);
                sb.Append(">");
                sb.Append((char)j);
                sb.Append("=");
                sb.Append(umlaute[rand.Next(umlaute.Length)]);
                sb.Append("<");
            }
            sb.Append("äöü\uD840\uDC00");
            return sb.ToString();
        }


        /**
         * Tests with strange characters in keys and data (Umlaute etc.)
         *
         */
        [Test]
        public void TestThree()
        {

            String company = Strangize("company");
            String manager = Strangize("manager");
            String category = Strangize("category");
            String title = Strangize("title");
            String author = Strangize("author");
            String comments = Strangize("comments");
            String keywords = Strangize("keywords");
            String subject = Strangize("subject");
            String p1 = Strangize("p1");
            String p2 = Strangize("p2");
            String k1 = Strangize("k1");
            String k2 = Strangize("k2");

            dsi.Company = company;
            dsi.Manager = manager;
            dsi.Category = category;

            si.Title = title;
            si.Author = author;
            si.Comments = comments;
            si.Keywords = keywords;
            si.Subject = subject;
            CustomProperties customProperties = dsi.CustomProperties;
            if (customProperties == null)
            {
                customProperties = new CustomProperties();
            }

            /* Insert some custom properties into the container. */
            customProperties.Put(k1, p1);
            customProperties.Put(k2, p2);
            customProperties.Put("Sample Number", 12345);
            customProperties.Put("Sample Boolean", false);
            DateTime date = new DateTime(1984,5,16,8,23,15);
            customProperties.Put("Sample Date", date);

            dsi.CustomProperties = customProperties;


            CloseAndReOpen();

            Assert.IsNotNull(dsi);
            Assert.IsNotNull(si);
            /* Change the category to "POI example". Any former category value will
             * be lost. If there has been no category yet, it will be Created. */
            Assert.AreEqual(category, dsi.Category, "Category");
            Assert.AreEqual(company, dsi.Company, "Company");
            Assert.AreEqual(manager, dsi.Manager, "Manager");

            Assert.AreEqual(author, si.Author);
            Assert.AreEqual(title, si.Title);
            Assert.AreEqual(comments, si.Comments);
            Assert.AreEqual(keywords, si.Keywords);
            Assert.AreEqual(subject, si.Subject);


            /* Read the custom properties. If there are no custom properties yet,
             * the application has to Create a new CustomProperties object. It will
             * serve as a container for custom properties. */
            customProperties = dsi.CustomProperties;
            if (customProperties == null)
            {
                Assert.Fail();
            }

            /* Insert some custom properties into the container. */
            // System.out.println(k1);
            String a1 = (String)customProperties[k1];
            Assert.AreEqual(p1, a1, "Key1");
            String a2 = (String)customProperties[k2];
            Assert.AreEqual(p2, a2, "Schlüsel2");
            int a3 = (int)customProperties["Sample Number"];
            Assert.AreEqual(12345, a3, "Sample Number");
            Boolean a4 = (Boolean)customProperties["Sample Boolean"];
            Assert.AreEqual(false, a4, "Sample Boolean");
            DateTime a5 = (DateTime)customProperties["Sample Date"];
            Assert.AreEqual(date, a5, "Custom Date:");


        }

        /**
         * Iterative Testing: writing, Reading etc.
         *
         */
        [Test]
        public void TestFour()
        {
            for (int i = 1; i < 100; i++)
            {
                SetUp();
                TestThree();
                TearDown();
            }
        }



        /**
         * Adds strange characters to the string with the Adding of unicode characters
         * @param s Input String
         * @return  the multiplied String
         */
        public String StrangizeU(String s)
        {

            StringBuilder sb = new StringBuilder();
            String[] umlaute = {"d","|","v","\\","$","V","\\","I","V","@","g","&"};
            int j = 0;
            Random rand = new Random();
            for (int i = 0; i < 5; i++)
            {
                sb.Append(s);
                sb.Append(" ");
                j = (char)rand.Next(220);
                j += 33;
                // System.out.println(j);
                sb.Append(">");
                sb.Append((char)j);
                sb.Append("=");
                sb.Append(umlaute[rand.Next(umlaute.Length)]);
                sb.Append("<");
            }
            sb.Append("漩黒uD840\uDC00");
            return sb.ToString();
        }
        /**
         * Unicode Test
         *
         */
        [Test]
        public void TestUnicode()
        {
            String company = StrangizeU("company");
            String manager = StrangizeU("manager");
            String category = StrangizeU("category");
            String title = StrangizeU("title");
            String author = StrangizeU("author");
            String comments = StrangizeU("comments");
            String keywords = StrangizeU("keywords");
            String subject = StrangizeU("subject");
            String p1 = StrangizeU("p1");
            String p2 = StrangizeU("p2");
            String k1 = StrangizeU("k1");
            String k2 = StrangizeU("k2");

            dsi.Company = company;
            dsi.Manager = manager;
            dsi.Category = category;

            si.Title = title;
            si.Author = author;
            si.Comments = comments;
            si.Keywords = keywords;
            si.Subject = subject;
            CustomProperties customProperties = dsi.CustomProperties;
            if (customProperties == null)
            {
                customProperties = new CustomProperties();
            }

            /* Insert some custom properties into the container. */
            customProperties.Put(k1, p1);
            customProperties.Put(k2, p2);
            customProperties.Put("Sample Number", 12345);
            customProperties.Put("Sample Boolean", true);
            DateTime date = new DateTime(1984, 5, 16, 8, 23, 15);
            customProperties.Put("Sample Date", date);

            dsi.CustomProperties = customProperties;


            CloseAndReOpen();

            Assert.IsNotNull(dsi);
            Assert.IsNotNull(si);
            /* Change the category to "POI example". Any former category value will
             * be lost. If there has been no category yet, it will be Created. */
            Assert.AreEqual(category, dsi.Category, "Category");
            Assert.AreEqual(company, dsi.Company, "Company");
            Assert.AreEqual(manager, dsi.Manager, "Manager");

            Assert.AreEqual(author, si.Author, "");
            Assert.AreEqual(title, si.Title, "");
            Assert.AreEqual(comments, si.Comments, "");
            Assert.AreEqual(keywords, si.Keywords, "");
            Assert.AreEqual(subject, si.Subject, "");


            /* Read the custom properties. If there are no custom properties yet,
             * the application has to Create a new CustomProperties object. It will
             * serve as a container for custom properties. */
            customProperties = dsi.CustomProperties;
            if (customProperties == null)
            {
                Assert.Fail();
            }

            /* Insert some custom properties into the container. */
            // System.out.println(k1);
            String a1 = (String)customProperties[k1];
            Assert.AreEqual(p1, a1, "Key1");
            String a2 = (String)customProperties[k2];
            Assert.AreEqual(p2, a2, "Schlüsel2");
            int a3 = (int)customProperties["Sample Number"];
            Assert.AreEqual(12345, a3, "Sample Number");
            Boolean a4 = (Boolean)customProperties["Sample Boolean"];
            Assert.AreEqual(true, a4, "Sample Boolean");
            DateTime a5 = (DateTime)customProperties["Sample Date"];
            Assert.AreEqual(date, a5, "Custom Date:");



        }


        /**
         * Iterative Testing of the unicode Test
         *
         */
        [Test]
        public void TestSix()
        {
            for (int i = 1; i < 100; i++)
            {
                SetUp();
                TestUnicode();
                TearDown();
            }
        }


        /**
         * Tests conversion in custom fields and errors
         *
         */
        [Test]
        public void TestConvAndExistance()
        {


            CustomProperties customProperties = dsi.CustomProperties;
            if (customProperties == null)
            {
                customProperties = new CustomProperties();
            }

            /* Insert some custom properties into the container. */
            customProperties.Put("int", 12345);
            customProperties.Put("negint", -12345);
            customProperties.Put("long", (long)12345);
            customProperties.Put("neglong", (long)-12345);
            customProperties.Put("bool", true);
            customProperties.Put("string", "a String");
            //customProperties.Put("float", new Float(12345.0)));  is not valid
            //customProperties.Put("negfloat", new Float(-12345.1))); is not valid
            customProperties.Put("double", (double)12345.2);
            customProperties.Put("negdouble", (double)-12345.3);
            //customProperties.Put("char", new Character('a'))); is not valid

            DateTime date = new DateTime(1984, 5, 16, 8, 23, 15);
            customProperties.Put("date", date);

            dsi.CustomProperties = customProperties;


            CloseAndReOpen();

            Assert.IsNotNull(dsi);
            Assert.IsNotNull(si);
            /* Change the category to "POI example". Any former category value will
             * be lost. If there has been no category yet, it will be Created. */
            Assert.IsNull(dsi.Category);
            Assert.IsNull(dsi.Company);
            Assert.IsNull(dsi.Manager);

            Assert.IsNull(si.Author);
            Assert.IsNull(si.Title);
            Assert.IsNull(si.Comments);
            Assert.IsNull(si.Keywords);
            Assert.IsNull(si.Subject);


            /* Read the custom properties. If there are no custom properties
             * yet, the application has to Create a new CustomProperties object.
             * It will serve as a container for custom properties. */
            customProperties = dsi.CustomProperties;
            if (customProperties == null)
            {
                Assert.Fail();
            }

            /* Insert some custom properties into the container. */

            int a3 = (int)customProperties["int"];
            Assert.AreEqual(12345, a3, "int");

            a3 = (int)customProperties["negint"];
            Assert.AreEqual(-12345, a3, "negint");

            long al = (long)customProperties["neglong"];
            Assert.AreEqual(-12345, al, "neglong");

            al = (long)customProperties["long"];
            Assert.AreEqual(12345, al, "long");

            Boolean a4 = (Boolean)customProperties["bool"];
            Assert.AreEqual(true, a4, "bool");

            DateTime a5 = (DateTime)customProperties["date"];
            Assert.AreEqual(date, a5, "Custom Date:");

            Double d = (Double)customProperties["double"];
            Assert.AreEqual(12345.2, d, "int");

            d = (Double)customProperties["negdouble"];
            Assert.AreEqual(-12345.3, d, "string");

            String s = (String)customProperties["string"];
            Assert.AreEqual("a String", s, "sring");

            Object o = null;

            o = customProperties["string"];
            if (!(o is String))
            {
                Assert.Fail();
            }
            o = customProperties["bool"];
            if (!(o is Boolean))
            {
                Assert.Fail();
            }

            o = customProperties["int"];
            if (!(o is int))
            {
                Assert.Fail();
            }
            o = customProperties["negint"];
            if (!(o is int))
            {
                Assert.Fail();
            }

            o = customProperties["long"];
            if (!(o is long))
            {
                Assert.Fail();
            }
            o = customProperties["neglong"];
            if (!(o is long))
            {
                Assert.Fail();
            }

            o = customProperties["double"];
            if (!(o is Double))
            {
                Assert.Fail();
            }
            o = customProperties["negdouble"];
            if (!(o is Double))
            {
                Assert.Fail();
            }

            o = customProperties["date"];
            if (!(o is DateTime))
            {
                Assert.Fail();
            }
        }
    }
}