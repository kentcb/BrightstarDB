namespace BrightstarDB.CodeGeneration.Tests
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using BrightstarDB.EntityFramework;
    using BrightstarDB.Rdf;
    using NUnit.Framework;

    [TestFixture]
    public class GeneratorTests
    {
        [Test]
        [TestCase(new[] { typeof(IEmptyEntity) }, "EmptyEntity.txt")]
        [TestCase(new[] { typeof(ISupportedScalarPropertyTypes) }, "SupportedScalarPropertyTypes.txt")]
        [TestCase(new[] { typeof(IAttributePropagation) }, "AttributePropagation.txt")]
        [TestCase(
            new[]
            {
                typeof(IIdentifierPrecedence_IdentifierAttributeTrumpsAll),
                typeof(IIdentifierPrecedence_ClassIdTrumpsId),
                typeof(IIdentifierPrecedence_Base),
                typeof(IIdentifierPrecedence_IdTrumpsInheritedId),
                typeof(IIdentifierPrecedence_CanInheritId)
            },
            "ExpectedIdentifierPrecedence.txt")]
        [TestCase(
            new[]
            {
                typeof(IRelationships_OneToOneA),
                typeof(IRelationships_OneToOneB),
                typeof(IRelationships_OneToManyA),
                typeof(IRelationships_OneToManyB),
                typeof(IRelationships_ManyToManyA),
                typeof(IRelationships_ManyToManyB),
                typeof(IRelationships_Reflexive)
            },
            "Relationships.txt")]
        public async Task TestCodeGeneration(Type[] entityInterfaces, string expectedCodeResource)
        {
            var results = await Generator
                .GenerateMocksAsync(
                    Language.CSharp,
                    @"..\..\..\core.sln",
                    "BrightstarDB.CodeGeneration.Tests",
                    interfacePredicate: x => entityInterfaces.Any(y => x.ToDisplayString() == y.FullName.Replace('+', '.')));
            var result = results
                .Aggregate(
                    new StringBuilder(),
                    (current, next) => current.AppendLine(next.ToString()),
                    x => x.ToString());

            using (var stream = this.GetType().Assembly.GetManifestResourceStream("BrightstarDB.CodeGeneration.Tests." + expectedCodeResource))
            using (var streamReader = new StreamReader(stream))
            {
                var expectedCode = streamReader.ReadToEnd();

                // make sure version changes don't break the tests
                expectedCode = expectedCode.Replace("$VERSION$", typeof(BrightstarException).Assembly.GetName().Version.ToString());

                //// useful when converting generated code to something that can be pasted into an expectation file
                //var sanitisedResult = result.Replace("1.10.0.0", "$VERSION$");
                //Debug.WriteLine(sanitisedResult);

                Assert.AreEqual(expectedCode, result);
            }
        }

        [Test]
        [TestCase(new[] { typeof(IInvalidIdType) }, "The property 'BrightstarDB.CodeGeneration.Tests.GeneratorTests.IInvalidIdType.Id' must be of type string to be used as the identity property for an entity. If this property is intended to be the identity property for the entity please change its type to string. If it is not intended to be the identity property, either rename this property or create an identity property and decorate it with the [BrightstarDB.EntityFramework.IdentifierAttribute] attribute.")]
        [TestCase(new[] { typeof(IIdWithSetter) }, "The property 'BrightstarDB.CodeGeneration.Tests.GeneratorTests.IIdWithSetter.Id' must not have a setter to be used as the identity property for an entity. If this property is intended to be the identity property for the entity please remove the setter. If it is not intended to be the identity property, either rename this property or create an identity propertyn and decorate it with the [BrightstarDB.EntityFramework.IdentifierAttribute] attribute.")]
        [TestCase(new[] { typeof(IPropertyWithUnsupportedType) }, "Invalid property: BrightstarDB.CodeGeneration.Tests.GeneratorTests.IPropertyWithUnsupportedType.Property - the property type BrightstarDB.CodeGeneration.Tests.GeneratorTests is not supported by Entity Framework.")]
        [TestCase(
            new[]
            {
                typeof(IInvalidInversePropertyName_A),
                typeof(IInvalidInversePropertyName_B)
            },
            "Invalid BrightstarDB.EntityFramework.InversePropertyAttribute attribute on property BrightstarDB.CodeGeneration.Tests.GeneratorTests.IInvalidInversePropertyName_B.A. A property named 'B' cannot be found on the target interface type BrightstarDB.CodeGeneration.Tests.GeneratorTests.IInvalidInversePropertyName_A.")]
        public async Task TestErrorConditions(Type[] entityInterfaces, string expectedErrorMessage)
        {
            try
            {
                var results = await Generator
                    .GenerateMocksAsync(
                        Language.CSharp,
                        @"..\..\..\core.sln",
                        "BrightstarDB.CodeGeneration.Tests",
                        interfacePredicate: x => entityInterfaces.Any(y => x.ToDisplayString() == y.FullName.Replace('+', '.')));

                Assert.Fail("No exception was thrown during code generation.");
            }
            catch (Exception ex)
            {
                Assert.AreEqual(expectedErrorMessage, ex.Message);
            }
        }

        #region EmptyEntity

        private interface IEmptyEntity
        {
        }

        #endregion

        #region IdentifierPrecedence

        private interface IIdentifierPrecedence_IdentifierAttributeTrumpsAll
        {
            [Identifier]
            string Something
            {
                get;
            }

            string IdentifierPrecedence_IdentifierAttributeTrumpsAllId
            {
                get;
            }

            string Id
            {
                get;
            }
        }

        private interface IIdentifierPrecedence_ClassIdTrumpsId
        {
            string IdentifierPrecedence_IdentifierAttributeTrumpsAllId
            {
                get;
            }

            string Id
            {
                get;
            }
        }

        private interface IIdentifierPrecedence_Base
        {
            [Identifier]
            string SomeId
            {
                get;
            }
        }

        private interface IIdentifierPrecedence_IdTrumpsInheritedId : IIdentifierPrecedence_Base
        {
            string Id
            {
                get;
            }
        }

        private interface IIdentifierPrecedence_CanInheritId : IIdentifierPrecedence_Base
        {
        }

        #endregion

        #region SupportedScalarPropertyTypes

        private interface ISupportedScalarPropertyTypes
        {
            string Id
            {
                get;
            }

            bool Bool
            {
                get;
                set;
            }

            short Int16
            {
                get;
                set;
            }

            int Int32
            {
                get;
                set;
            }

            long Int64
            {
                get;
                set;
            }

            ushort UInt16
            {
                get;
                set;
            }

            uint UInt32
            {
                get;
                set;
            }

            ulong UInt64
            {
                get;
                set;
            }

            string String
            {
                get;
                set;
            }

            decimal Decimal
            {
                get;
                set;
            }

            double Double
            {
                get;
                set;
            }

            float Single
            {
                get;
                set;
            }

            byte Byte
            {
                get;
                set;
            }

            char Char
            {
                get;
                set;
            }

            sbyte SByte
            {
                get;
                set;
            }

            DateTime DateTime
            {
                get;
                set;
            }

            Guid Guid
            {
                get;
                set;
            }

            Uri Uri
            {
                get;
                set;
            }

            PlainLiteral PlainLiteral
            {
                get;
                set;
            }

            DayOfWeek Enumeration
            {
                get;
                set;
            }

            byte[] ByteArray
            {
                get;
                set;
            }

            bool? NullableBool
            {
                get;
                set;
            }

            short? NullableInt16
            {
                get;
                set;
            }

            int? NullableInt32
            {
                get;
                set;
            }

            long? NullableInt64
            {
                get;
                set;
            }

            ushort? NullableUInt16
            {
                get;
                set;
            }

            uint? NullableUInt32
            {
                get;
                set;
            }

            ulong? NullableUInt64
            {
                get;
                set;
            }

            decimal? NullableDecimal
            {
                get;
                set;
            }

            double? NullableDouble
            {
                get;
                set;
            }

            float? NullableSingle
            {
                get;
                set;
            }

            byte? NullableByte
            {
                get;
                set;
            }

            char? NullableChar
            {
                get;
                set;
            }

            sbyte? NullableSByte
            {
                get;
                set;
            }

            DateTime? NullableDateTime
            {
                get;
                set;
            }

            Guid? NullableGuid
            {
                get;
                set;
            }

            DayOfWeek? NullableEnumeration
            {
                get;
                set;
            }
        }


        #endregion

        #region Relationships

        private interface IRelationships_OneToOneA
        {
            IRelationships_OneToOneB B
            {
                get;
                set;
            }
        }

        private interface IRelationships_OneToOneB
        {
            [InverseProperty("B")]
            IRelationships_OneToOneA A
            {
                get;
                set;
            }
        }

        private interface IRelationships_OneToManyA
        {
            ICollection<IRelationships_OneToManyB> Bs
            {
                get;
                set;
            }
        }

        private interface IRelationships_OneToManyB
        {
            [InverseProperty("Bs")]
            IRelationships_OneToManyA A
            {
                get;
                set;
            }
        }

        private interface IRelationships_ManyToManyA
        {
            ICollection<IRelationships_ManyToManyB> Bs
            {
                get;
                set;
            }
        }

        private interface IRelationships_ManyToManyB
        {
            [InverseProperty("Bs")]
            ICollection<IRelationships_ManyToManyA> As
            {
                get;
                set;
            }
        }

        private interface IRelationships_Reflexive
        {
            ICollection<IRelationships_Reflexive> As
            {
                get;
                set;
            }

            [InverseProperty("As")]
            ICollection<IRelationships_Reflexive> Bs
            {
                get;
                set;
            }
        }

        #endregion

        #region Attribute Propagation

        private interface IAttributePropagation
        {
            [Identifier]
            [Browsable(true)]
            string Id
            {
                get;
            }

            [Browsable(true)]
            [System.ComponentModel.Description("This should carry through.")]
            int Number
            {
                get;
                set;
            }
        }

        #endregion

        #region InvalidIdType

        private interface IInvalidIdType
        {
            [Identifier]
            int Id
            {
                get;
            }
        }

        #endregion

        #region IdWithSetter

        private interface IIdWithSetter
        {
            [Identifier]
            string Id
            {
                get;
                set;
            }
        }

        #endregion

        #region PropertyWithUnsupportedType

        private interface IPropertyWithUnsupportedType
        {
            GeneratorTests Property
            {
                get;
                set;
            }
        }

        #endregion

        #region InvalidInversePropertyName

        private interface IInvalidInversePropertyName_A
        {
            ICollection<IInvalidInversePropertyName_B> Bs
            {
                get;
                set;
            }
        }

        private interface IInvalidInversePropertyName_B
        {
            [InverseProperty("B")]
            IInvalidInversePropertyName_A A
            {
                get;
                set;
            }
        }

        #endregion
    }
}