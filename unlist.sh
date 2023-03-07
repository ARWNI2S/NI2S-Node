version=$1
nuget delete NI2SNode.Client "$version" -source nuget.org -NonInteractive
nuget delete NI2SNode.Core "$version" -source nuget.org -NonInteractive
nuget delete NI2SNode.Abstractions "$version" -source nuget.org -NonInteractive
nuget delete NI2SNode.Protocol "$version" -source nuget.org -NonInteractive