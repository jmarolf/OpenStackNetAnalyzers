﻿namespace OpenStackNetAnalyzers
{
    using System;
    using Microsoft.CodeAnalysis;

    internal static class SdkTypeSymbolExtensions
    {
        private const string FullyQualifiedExtensibleJsonObject = "global::OpenStack.ObjectModel.ExtensibleJsonObject";

        private const string FullyQualifiedDelegatingHttpApiCallT = "global::OpenStack.Net.DelegatingHttpApiCall<T>";

        private const string FullyQualifiedIHttpService = "global::OpenStack.Services.IHttpService";

        public static bool IsExtensibleJsonObject(this INamedTypeSymbol symbol)
        {
            while (symbol != null && symbol.SpecialType != SpecialType.System_Object)
            {
                if (string.Equals(FullyQualifiedExtensibleJsonObject, symbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat), StringComparison.Ordinal))
                    return true;

                symbol = symbol.BaseType;
            }

            return false;
        }

        public static bool IsDelegatingHttpApiCall(this INamedTypeSymbol symbol)
        {
            while (symbol != null && symbol.SpecialType != SpecialType.System_Object)
            {
                if (symbol.IsGenericType)
                {
                    var originalDefinition = symbol.OriginalDefinition;
                    string fullyQualifiedName = originalDefinition.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
                    if (string.Equals(FullyQualifiedDelegatingHttpApiCallT, fullyQualifiedName, StringComparison.Ordinal))
                        return true;
                }

                symbol = symbol.BaseType;
            }

            return false;
        }

        public static bool IsHttpServiceInterface(this INamedTypeSymbol symbol)
        {
            if (symbol == null || symbol.TypeKind != TypeKind.Interface)
                return false;

            foreach (INamedTypeSymbol interfaceType in symbol.AllInterfaces)
            {
                if (string.Equals(FullyQualifiedIHttpService, interfaceType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)))
                    return true;
            }

            return false;
        }
    }
}
