using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
#if UNITY_XCODE_API_BUILD
using UnityEditor.iOS.Xcode;
#else
using UnityEditor.iOS.Xcode.Custom;
#endif

namespace UnityEditor.iOS.Xcode.Tests
{

    [TestFixture]
    public class AssetCatalogTests : GenericTester
    {
        public AssetCatalogTests() : base("AssetCatalogTestFiles", "AssetCatalogTestOutput", false /*true for debug*/)
        {
        }

        static void AssertFileExistsAndHasContents(string path, string contents)
        {
            Assert.IsTrue(File.Exists(path));
            Assert.AreEqual(contents, File.ReadAllText(path));
        }

        [Test]
        public void DataSetCreationWorks()
        {
            var testFiles = new TestFileCache(Path.Combine(GetTestOutputPath(), "Dataset1Files"));

            string catalogPath = Path.Combine(GetTestOutputPath(), "Dataset1.xcassets");
            var catalog = new AssetCatalog(catalogPath, "test.test");
            var dataset = catalog.OpenDataSet("data1");

            dataset.AddVariant(new DeviceRequirement().AddDevice(DeviceTypeRequirement.iPad),
                               testFiles.CreateFile("data1.dat", "data1"), null);

            dataset.AddVariant(new DeviceRequirement().AddMemory(MemoryRequirement.Mem1GB),
                               testFiles.CreateFile("data2.dat", "data2"), "testtype1");

            dataset.AddVariant(new DeviceRequirement().AddMemory(MemoryRequirement.Mem2GB).AddGraphics(GraphicsRequirement.Metal1v2),
                               testFiles.CreateFile("data3", "data3"), "testtype2");

            catalog.Write();

            string datasetPath = Path.Combine(catalogPath, "data1.dataset");
            Assert.IsTrue(Directory.Exists(catalogPath));
            Assert.IsTrue(Directory.Exists(datasetPath));
            AssertFileExistsAndHasContents(Path.Combine(datasetPath, "Contents.json"),
                                           File.ReadAllText(Path.Combine(GetTestSourcePath(), "Dataset1.Contents.json")));
            AssertFileExistsAndHasContents(Path.Combine(datasetPath, "data1.dat"), "data1");
            AssertFileExistsAndHasContents(Path.Combine(datasetPath, "data2.dat"), "data2");
            AssertFileExistsAndHasContents(Path.Combine(datasetPath, "data3"), "data3");

            if (!DebugEnabled())
            {
                testFiles.CleanUp();
                Directory.Delete(catalogPath, true);
            }
        }

        [Test]
        public void ImageSetCreationWorks()
        {
            var testFiles = new TestFileCache(Path.Combine(GetTestOutputPath(), "Imageset1Files"));

            string catalogPath = Path.Combine(GetTestOutputPath(), "Imageset1.xcassets");
            var catalog = new AssetCatalog(catalogPath, "test.test");
            var imageset = catalog.OpenImageSet("img1");

            imageset.AddVariant(new DeviceRequirement().AddWidthClass(SizeClassRequirement.Regular),
                                testFiles.CreateFile("data1.png", "img1"));

            imageset.AddVariant(new DeviceRequirement().AddHeightClass(SizeClassRequirement.Compact),
                                testFiles.CreateFile("data2.png", "img2"));

            imageset.AddVariant(new DeviceRequirement().AddScale(ScaleRequirement.X1),
                                testFiles.CreateFile("data3.png", "img3"));

            var alignment = new ImageAlignment();
            alignment.top = 1;
            alignment.bottom = 2;
            alignment.left = 3;
            alignment.right = 4;

            var resizing = new ImageResizing();
            resizing.type = ImageResizing.SlicingType.HorizontalAndVertical;
            resizing.top = 1;
            resizing.bottom = 2;
            resizing.left = 3;
            resizing.right = 4;
            resizing.centerResizeMode = ImageResizing.ResizeMode.Stretch;
            resizing.centerWidth = 2;
            resizing.centerHeight = 4;

            imageset.AddVariant(new DeviceRequirement().AddScale(ScaleRequirement.X3),
                                testFiles.CreateFile("data4.png", "img4"), alignment, resizing);
            catalog.Write();

            string imagesetPath = Path.Combine(catalogPath, "img1.imageset");
            Assert.IsTrue(Directory.Exists(catalogPath));
            Assert.IsTrue(Directory.Exists(imagesetPath));
            AssertFileExistsAndHasContents(Path.Combine(imagesetPath, "Contents.json"),
                                           File.ReadAllText(Path.Combine(GetTestSourcePath(), "Imageset1.Contents.json")));
            AssertFileExistsAndHasContents(Path.Combine(imagesetPath, "data1.png"), "img1");
            AssertFileExistsAndHasContents(Path.Combine(imagesetPath, "data2.png"), "img2");
            AssertFileExistsAndHasContents(Path.Combine(imagesetPath, "data3.png"), "img3");
            AssertFileExistsAndHasContents(Path.Combine(imagesetPath, "data4.png"), "img4");

            if (!DebugEnabled())
            {
                testFiles.CleanUp();
                Directory.Delete(catalogPath, true);
            }
        }

        [Test]
        public void ImageSetCanBeCreatedWithDuplicateFiles()
        {
            var testFiles = new TestFileCache(Path.Combine(GetTestOutputPath(), "Imageset1Files"));

            string catalogPath = Path.Combine(GetTestOutputPath(), "Imageset-duplicate.xcassets");
            var catalog = new AssetCatalog(catalogPath, "test.test");
            var imageset = catalog.OpenImageSet("img1");

            var filePath = testFiles.CreateFile("data1.png", "img1");
            imageset.AddVariant(new DeviceRequirement().AddWidthClass(SizeClassRequirement.Regular),
                                filePath);

            imageset.AddVariant(new DeviceRequirement().AddHeightClass(SizeClassRequirement.Compact),
                                filePath);

            catalog.Write();

            string imagesetPath = Path.Combine(catalogPath, "img1.imageset");
            Assert.IsTrue(Directory.Exists(catalogPath));
            Assert.IsTrue(Directory.Exists(imagesetPath));
            AssertFileExistsAndHasContents(Path.Combine(imagesetPath, "Contents.json"),
                                           File.ReadAllText(Path.Combine(GetTestSourcePath(), "Imageset-duplicate.Contents.json")));
            AssertFileExistsAndHasContents(Path.Combine(imagesetPath, "data1.png"), "img1");
            AssertFileExistsAndHasContents(Path.Combine(imagesetPath, "data1-1.png"), "img1");

            if (!DebugEnabled())
            {
                testFiles.CleanUp();
                Directory.Delete(catalogPath, true);
            }
        }

        [Test]
        public void FoldersAreCreatedAsExpected()
        {
            var testFiles = new TestFileCache(Path.Combine(GetTestOutputPath(), "Folder1Files"));

            string catalogPath = Path.Combine(GetTestOutputPath(), "Folder1.xcassets");
            var catalog = new AssetCatalog(catalogPath, "test.test");
            var dataset = catalog.OpenDataSet("test/test/test2/data1");

            dataset.AddVariant(new DeviceRequirement().AddDevice(DeviceTypeRequirement.iPad),
                               testFiles.CreateFile("data1.dat", "data1"), null);

            var folder = catalog.OpenFolder("test/test2");
            folder.providesNamespace = true;
            dataset = catalog.OpenDataSet("test/test2/data2");
            dataset.AddVariant(new DeviceRequirement().AddDevice(DeviceTypeRequirement.iPad),
                               testFiles.CreateFile("data2.dat", "data2"), null);

            catalog.Write();

            Assert.IsTrue(Directory.Exists(catalogPath));
            Assert.IsFalse(File.Exists(Path.Combine(catalogPath, "Contents.json")));
            Assert.IsTrue(Directory.Exists(Path.Combine(catalogPath, "test")));
            Assert.IsFalse(File.Exists(Path.Combine(catalogPath, "test/Contents.json")));
            Assert.IsTrue(Directory.Exists(Path.Combine(catalogPath, "test/test")));
            Assert.IsFalse(File.Exists(Path.Combine(catalogPath, "test/test/Contents.json")));
            Assert.IsTrue(Directory.Exists(Path.Combine(catalogPath, "test/test/test2")));
            Assert.IsFalse(File.Exists(Path.Combine(catalogPath, "test/test/test2/Contents.json")));
            Assert.IsTrue(Directory.Exists(Path.Combine(catalogPath, "test/test/test2/data1.dataset")));
            Assert.IsTrue(Directory.Exists(Path.Combine(catalogPath, "test/test2")));
            AssertFileExistsAndHasContents(Path.Combine(catalogPath, "test/test2/Contents.json"),
                                           File.ReadAllText(Path.Combine(GetTestSourcePath(), "Folder1.test2.Contents.json")));
            Assert.IsTrue(Directory.Exists(Path.Combine(catalogPath, "test/test2/data2.dataset")));
            AssertFileExistsAndHasContents(Path.Combine(catalogPath, "test/test/test2/data1.dataset/Contents.json"),
                                           File.ReadAllText(Path.Combine(GetTestSourcePath(), "Folder1.data1.dataset.Contents.json")));
            AssertFileExistsAndHasContents(Path.Combine(catalogPath, "test/test2/data2.dataset/Contents.json"),
                                           File.ReadAllText(Path.Combine(GetTestSourcePath(), "Folder1.data2.dataset.Contents.json")));

            if (!DebugEnabled())
            {
                testFiles.CleanUp();
                Directory.Delete(catalogPath, true);
            }
        }

        [Test]
        public void ODRTagsAreCreatedAsExpected()
        {
            var testFiles = new TestFileCache(Path.Combine(GetTestOutputPath(), "ODRTags1Files"));

            string catalogPath = Path.Combine(GetTestOutputPath(), "ODRTags1.xcassets");
            var catalog = new AssetCatalog(catalogPath, "test.test");
            var dataset = catalog.OpenDataSet("data1");

            dataset.AddVariant(new DeviceRequirement(), testFiles.CreateFile("data1.dat", "data1"), null);
            dataset.AddOnDemandResourceTag("tag1");
            dataset.AddOnDemandResourceTag("tag2");

            catalog.Write();

            string datasetPath = Path.Combine(catalogPath, "data1.dataset");
            Assert.IsTrue(Directory.Exists(catalogPath));
            Assert.IsTrue(Directory.Exists(datasetPath));
            AssertFileExistsAndHasContents(Path.Combine(datasetPath, "Contents.json"),
                                           File.ReadAllText(Path.Combine(GetTestSourcePath(), "ODRTags1.data1.dataset.Contents.json")));
            AssertFileExistsAndHasContents(Path.Combine(datasetPath, "data1.dat"), "data1");

            if (!DebugEnabled())
            {
                testFiles.CleanUp();
                Directory.Delete(catalogPath, true);
            }
        }

        [Test]
        public void ImageStackCreationWorks()
        {
            var testFiles = new TestFileCache(Path.Combine(GetTestOutputPath(), "ImageStack1Files"));

            string catalogPath = Path.Combine(GetTestOutputPath(), "ImageStack1.xcassets");
            var catalog = new AssetCatalog(catalogPath, "test.test");
            var imageStack = catalog.OpenImageStack("stack1");

            var layer = imageStack.AddLayer("layer1");
            var imageset = layer.GetImageSet();
            imageset.AddVariant(new DeviceRequirement().AddWidthClass(SizeClassRequirement.Regular),
                                testFiles.CreateFile("data1.png", "img1"));

            imageset.AddVariant(new DeviceRequirement().AddHeightClass(SizeClassRequirement.Compact),
                                testFiles.CreateFile("data2.png", "img2"));

            layer = imageStack.AddLayer("layer2");
            layer.SetReference("Image1");
            catalog.Write();

            string imageStackPath = Path.Combine(catalogPath, "stack1.imagestack");
            string layer1Path = Path.Combine(imageStackPath, "layer1.imagestacklayer");
            string layer1ImageSetPath = Path.Combine(layer1Path, "Content.imageset");
            string layer2Path = Path.Combine(imageStackPath, "layer2.imagestacklayer");
            Assert.IsTrue(Directory.Exists(catalogPath));
            Assert.IsTrue(Directory.Exists(imageStackPath));
            Assert.IsTrue(Directory.Exists(layer1Path));
            Assert.IsTrue(Directory.Exists(layer1ImageSetPath));
            Assert.IsTrue(Directory.Exists(layer2Path));

            AssertFileExistsAndHasContents(Path.Combine(imageStackPath, "Contents.json"),
                                           File.ReadAllText(Path.Combine(GetTestSourcePath(), "ImageStack1.Contents.json")));
            AssertFileExistsAndHasContents(Path.Combine(layer1Path, "Contents.json"),
                                           File.ReadAllText(Path.Combine(GetTestSourcePath(), "ImageStack1.layer1.Contents.json")));
            AssertFileExistsAndHasContents(Path.Combine(layer1ImageSetPath, "Contents.json"),
                                           File.ReadAllText(Path.Combine(GetTestSourcePath(), "ImageStack1.layer1.Content.Contents.json")));
            AssertFileExistsAndHasContents(Path.Combine(layer2Path, "Contents.json"),
                                           File.ReadAllText(Path.Combine(GetTestSourcePath(), "ImageStack1.layer2.Contents.json")));
            AssertFileExistsAndHasContents(Path.Combine(layer1ImageSetPath, "data1.png"), "img1");
            AssertFileExistsAndHasContents(Path.Combine(layer1ImageSetPath, "data2.png"), "img2");

            if (!DebugEnabled())
            {
                testFiles.CleanUp();
                Directory.Delete(catalogPath, true);
            }
        }
        
        [Test]
        public void BrandAssetCreationWorks()
        {
            var testFiles = new TestFileCache(Path.Combine(GetTestOutputPath(), "BrandAsset11Files"));
            
            string catalogPath = Path.Combine(GetTestOutputPath(), "BrandAsset1.xcassets");
            var catalog = new AssetCatalog(catalogPath, "test.test");
            var brandAssets = catalog.OpenBrandAssetGroup("brand1");

            var icon1 = brandAssets.OpenImageSet("icon1", "tv", "primary-app-icon", 128, 256);
            icon1.AddVariant(new DeviceRequirement(), testFiles.CreateFile("data1.png", "img1"));
            catalog.Write();
            
            string brandAssetsPath = Path.Combine(catalogPath, "brand1.brandassets");
            string icon1Path = Path.Combine(brandAssetsPath, "icon1.imageset");
            Assert.IsTrue(Directory.Exists(catalogPath));
            Assert.IsTrue(Directory.Exists(brandAssetsPath));
            Assert.IsTrue(Directory.Exists(icon1Path));
            
            AssertFileExistsAndHasContents(Path.Combine(brandAssetsPath, "Contents.json"),
                                           File.ReadAllText(Path.Combine(GetTestSourcePath(), "BrandAssets1.Contents.json")));
            AssertFileExistsAndHasContents(Path.Combine(icon1Path, "Contents.json"),
                                           File.ReadAllText(Path.Combine(GetTestSourcePath(), "BrandAssets1.icon1.Contents.json")));
            AssertFileExistsAndHasContents(Path.Combine(icon1Path, "data1.png"), "img1");
            
            if (!DebugEnabled())
            {
                testFiles.CleanUp();
                Directory.Delete(catalogPath, true);
            }
        }
    }

} // namespace UnityEditor.iOS.Xcode
