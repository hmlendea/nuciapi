using System.Collections.Generic;
using System.Linq;

using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using ArchUnitNET.Fluent.Syntax.Elements.Types;
using ArchUnitNET.Loader;
using ArchUnitNET.NUnit;

using NUnit.Framework;

using NuciAPI.Requests;
using NuciAPI.Responses;

namespace NuciAPI.ArchitectureTests
{
    /// <summary>
    /// Dependency and boundary tests over the loaded production architecture.
    /// </summary>
    [TestFixture]
    public class ArchitectureTests
    {
        private static readonly ArchUnitNET.Domain.Architecture LoadedArchitecture = BuildArchitecture();

        private static ArchUnitNET.Domain.Architecture BuildArchitecture()
            => new ArchLoader()
                .LoadAssemblyIncludingDependencies(typeof(NuciApiRequest).Assembly)
                .Build();

        [Test]
        public void GivenProductionAssemblies_WhenLoadingArchitecture_ThenRequestContractsAreFound()
        {
            IEnumerable<Class> requestTypes = LoadedArchitecture.Classes.Where(IsRequestContract);

            Assert.That(
                requestTypes,
                Is.Not.Empty,
                "No NuciAPI.Requests types were loaded; the production assembly inventory is incomplete.");
        }

        [Test]
        public void GivenProductionAssemblies_WhenLoadingArchitecture_ThenResponseContractsAreFound()
        {
            IEnumerable<Class> responseTypes = LoadedArchitecture.Classes.Where(IsResponseContract);

            Assert.That(
                responseTypes,
                Is.Not.Empty,
                "No NuciAPI.Responses types were loaded; the production assembly inventory is incomplete.");
        }

        [Test]
        public void GivenNuciApiContracts_WhenCheckingPersistenceDependencies_ThenNoneAreUsed()
        {
            ArchUnitNET.Fluent.IArchRule noPersistenceRule = ArchRuleDefinition.Types()
                .Should().NotDependOnAnyTypesThat().ResideInNamespaceMatching(@"Microsoft\.EntityFrameworkCore\..*")
                .AndShould().NotDependOnAnyTypesThat().ResideInNamespaceMatching(@"Amazon\.DynamoDBv2\..*")
                .Because("NuciAPI is a serialisation-agnostic contract library and must not depend on persistence frameworks.");

            IEnumerable<IType> contractTypes = LoadedArchitecture.Types
                .Where(definedType => definedType.Namespace.FullName.StartsWith("NuciAPI."));

            Assert.That(
                contractTypes,
                Is.Not.Empty,
                "Vacuous-selector guard: no NuciAPI types were selected for the persistence-dependency rule.");

            CheckRulePassed(noPersistenceRule);
        }

        [Test]
        public void GivenNuciApiContracts_WhenCheckingAspNetCoreDependencies_ThenNoneAreUsed()
        {
            ArchUnitNET.Fluent.IArchRule noAspNetCoreRule = ArchRuleDefinition.Types()
                .Should().NotDependOnAnyTypesThat().ResideInNamespaceMatching(@"Microsoft.AspNetCore\..*")
                .Because("NuciAPI must stay framework-agnostic and usable outside ASP.NET Core.");

            IEnumerable<IType> contractTypes = LoadedArchitecture.Types
                .Where(definedType => definedType.Namespace.FullName.StartsWith("NuciAPI."));

            Assert.That(
                contractTypes,
                Is.Not.Empty,
                "Vacuous-selector guard: no NuciAPI types were selected for the ASP.NET Core dependency rule.");

            CheckRulePassed(noAspNetCoreRule);
        }

        [Test]
        public void GivenNuciApiRequests_WhenCheckingNamespacePlacement_ThenTheyResideInNuciApiRequests()
        {
            GivenContractNamespace_WhenCheckingPlacement_ThenAllTypesResideInIt("NuciAPI.Requests");
        }

        [Test]
        public void GivenNuciApiResponses_WhenCheckingNamespacePlacement_ThenTheyResideInNuciApiResponses()
        {
            GivenContractNamespace_WhenCheckingPlacement_ThenAllTypesResideInIt("NuciAPI.Responses");
        }

        [Test]
        public void GivenNuciApiContracts_WhenCheckingIllegalNamespaces_ThenNoneAreUsed()
        {
            ArchUnitNET.Fluent.IArchRule noInterfacesNamespaceRule = ArchRuleDefinition.Types()
                .Should().NotResideInNamespaceMatching(@"NuciAPI\..*\.Interfaces")
                .Because("The '.Interfaces' namespace pattern is prohibited; interfaces must be colocated with their types.");

            IEnumerable<IType> contractTypes = LoadedArchitecture.Types
                .Where(definedType => definedType.Namespace.FullName.StartsWith("NuciAPI."));

            Assert.That(
                contractTypes,
                Is.Not.Empty,
                "Vacuous-selector guard: no NuciAPI types were selected for the namespace rule.");

            CheckRulePassed(noInterfacesNamespaceRule);
        }

        [Test]
        public void GivenNuciApiContracts_WhenCheckingForbiddenThirdPartyDependencies_ThenNoneAreUsed()
        {
            ArchUnitNET.Fluent.IArchRule noForbiddenThirdPartyRule = ArchRuleDefinition.Types()
                .Should().NotDependOnAnyTypesThat().ResideInNamespaceMatching(@"MediatR\..*")
                .AndShould().NotDependOnAnyTypesThat().ResideInNamespaceMatching(@"Dapper\..*")
                .Because("NuciAPI only permits the NuciSecurity.HMAC dependency.");

            IEnumerable<IType> contractTypes = LoadedArchitecture.Types
                .Where(definedType => definedType.Namespace.FullName.StartsWith("NuciAPI."));

            Assert.That(
                contractTypes,
                Is.Not.Empty,
                "Vacuous-selector guard: no NuciAPI types were selected for the third-party dependency rule.");

            CheckRulePassed(noForbiddenThirdPartyRule);
        }

        [Test]
        public void GivenNuciApiSuccessResponse_WhenCheckingInheritance_ThenItExtendsNuciApiResponse()
        {
            Class successResponse = LoadedArchitecture.Classes
                .First(definedType => definedType.FullName.Equals("NuciAPI.Responses.NuciApiSuccessResponse"));

            Assert.That(
                successResponse.BaseClass?.FullName,
                Is.EqualTo("NuciAPI.Responses.NuciApiResponse"),
                "NuciApiSuccessResponse must inherit NuciApiResponse to preserve the response contract hierarchy.");
        }

        [Test]
        public void GivenNuciApiErrorResponse_WhenCheckingInheritance_ThenItExtendsNuciApiResponse()
        {
            Class errorResponse = LoadedArchitecture.Classes
                .First(definedType => definedType.FullName.Equals("NuciAPI.Responses.NuciApiErrorResponse"));

            Assert.That(
                errorResponse.BaseClass?.FullName,
                Is.EqualTo("NuciAPI.Responses.NuciApiResponse"),
                "NuciApiErrorResponse must inherit NuciApiResponse to preserve the response contract hierarchy.");
        }

        [Test]
        public void GivenNuciApiContentResponse_WhenCheckingInheritance_ThenItExtendsNuciApiSuccessResponse()
        {
            Class contentResponse = LoadedArchitecture.Classes
                .First(definedType => definedType.FullName.Equals("NuciAPI.Responses.NuciApiContentResponse`1"));

            Assert.That(
                contentResponse.BaseClass?.FullName,
                Is.EqualTo("NuciAPI.Responses.NuciApiResponse"),
                "NuciApiContentResponse`1 must inherit NuciApiResponse to carry response content payloads.");
        }

        [Test]
        public void GivenNuciApiRequest_WhenCheckingDeclaration_ThenItIsAnAbstractClass()
        {
            Class request = LoadedArchitecture.Classes
                .First(definedType => definedType.FullName.Equals("NuciAPI.Requests.NuciApiRequest"));

            Assert.That(
                request.IsAbstract,
                Is.True,
                "NuciApiRequest must remain abstract: it is the base contract for all API requests.");
        }

        [Test]
        public void GivenNuciApiResponse_WhenCheckingDeclaration_ThenItIsAnAbstractClass()
        {
            Class response = LoadedArchitecture.Classes
                .First(definedType => definedType.FullName.Equals("NuciAPI.Responses.NuciApiResponse"));

            Assert.That(
                response.IsAbstract,
                Is.True,
                "NuciApiResponse must remain abstract: it is the base contract for all API responses.");
        }

        [Test]
        public void GivenNuciApiContractTypes_WhenCheckingVisibility_ThenTheyAreAllPublicOrNestedPublic()
        {
            IEnumerable<Class> internalContracts = LoadedArchitecture.Classes
                .Where(definedType => definedType.Namespace.FullName.StartsWith("NuciAPI."))
                .Where(definedType => definedType.Visibility != ArchUnitNET.Domain.Visibility.Public);

            Assert.That(
                internalContracts,
                Is.Empty,
                "All NuciAPI contract types must be public: they form the published NuGet surface.");
        }

        private static bool IsRequestContract(IType definedType)
            => definedType.Namespace.FullName.Equals("NuciAPI.Requests");

        private static bool IsResponseContract(IType definedType)
            => definedType.Namespace.FullName.Equals("NuciAPI.Responses");

        private static void GivenContractNamespace_WhenCheckingPlacement_ThenAllTypesResideInIt(string expectedNamespace)
        {
            ArchUnitNET.Fluent.IArchRule placementRule = ArchRuleDefinition.Types()
                .That().ResideInNamespace(expectedNamespace)
                .Should().Exist()
                .Because("Contract types must remain in their prescribed namespace.");

            IEnumerable<IType> namespaceTypes = LoadedArchitecture.Types
                .Where(definedType => definedType.Namespace.FullName.Equals(expectedNamespace));

            Assert.That(
                namespaceTypes,
                Is.Not.Empty,
                $"Vacuous-selector guard: no types were found in {expectedNamespace}.");

            CheckRulePassed(placementRule);
        }

        private static void CheckRulePassed(ArchUnitNET.Fluent.IArchRule archRule)
        {
            System.Collections.Generic.IEnumerable<ArchUnitNET.Fluent.EvaluationResult> failures = archRule
                .Evaluate(LoadedArchitecture)
                .Where(evaluationResult => !evaluationResult.Passed);

            Assert.That(
                failures,
                Is.Empty,
                "Architecture rule violated: " + archRule.Description);
        }
    }
}
