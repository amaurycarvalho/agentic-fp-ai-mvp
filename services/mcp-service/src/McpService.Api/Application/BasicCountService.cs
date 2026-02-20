using McpService.Api.Contracts;

namespace McpService.Api.Application;

public sealed class BasicCountService : IBasicCountService
{
    private static readonly string[] EiKeywords = ["cadastrar", "criar", "inserir", "registrar", "enviar", "atualizar"];
    private static readonly string[] EoKeywords = ["gerar", "emitir", "notificar", "exportar", "publicar"];
    private static readonly string[] EqKeywords = ["consultar", "buscar", "listar", "visualizar", "obter"];

    private static readonly string[] IlfKeywords = ["armazenar", "persistir", "banco", "tabela", "cadastro", "manter"];
    private static readonly string[] EifKeywords = ["externo", "terceiro", "api externa", "sistema externo", "integra"];

    public CountBasicResponse Analyze(CountBasicRequest request)
    {
        var normalized = request.UserStory.Trim().ToLowerInvariant();
        var auditTrail = new List<string>
        {
            "Normalized user story for deterministic keyword analysis."
        };

        var transactionalFunctions = ClassifyTransactions(normalized, auditTrail);
        if (transactionalFunctions.Count == 0)
        {
            transactionalFunctions.Add(new FunctionClassification("EQ", "Fallback classification: no explicit keyword found, defaulting to query."));
            auditTrail.Add("Applied fallback transaction classification: EQ.");
        }

        var dataFunctions = ClassifyDataFunctions(normalized, auditTrail);
        if (dataFunctions.Count == 0)
        {
            dataFunctions.Add(new FunctionClassification("ILF", "Fallback classification: no data-source keyword found, defaulting to internal logical file."));
            auditTrail.Add("Applied fallback data classification: ILF.");
        }

        var det = ResolveDet(request, normalized, auditTrail);
        var ftr = ResolveFtr(request, dataFunctions, auditTrail);
        var complexity = ResolveComplexity(det, ftr);
        var points = transactionalFunctions.Sum(tf => ResolveFunctionPoints(tf.Type, complexity));

        auditTrail.Add($"Complexity resolved by DETxFTR matrix: DET={det}, FTR={ftr}, Complexity={complexity}.");
        auditTrail.Add($"Total transactional function points: {points}.");

        return new CountBasicResponse(
            transactionalFunctions,
            dataFunctions,
            new ComplexitySummary(det, ftr, complexity, points),
            auditTrail);
    }

    private static List<FunctionClassification> ClassifyTransactions(string text, List<string> auditTrail)
    {
        var list = new List<FunctionClassification>();

        if (ContainsAny(text, EiKeywords))
        {
            list.Add(new FunctionClassification("EI", "Detected input/maintenance verbs (e.g., cadastrar/criar/enviar)."));
            auditTrail.Add("Transaction EI identified from input verb keyword match.");
        }

        if (ContainsAny(text, EoKeywords))
        {
            list.Add(new FunctionClassification("EO", "Detected output/reporting verbs (e.g., gerar/emitir/exportar)."));
            auditTrail.Add("Transaction EO identified from output verb keyword match.");
        }

        if (ContainsAny(text, EqKeywords))
        {
            list.Add(new FunctionClassification("EQ", "Detected query verbs (e.g., consultar/listar/buscar)."));
            auditTrail.Add("Transaction EQ identified from query verb keyword match.");
        }

        return list;
    }

    private static List<FunctionClassification> ClassifyDataFunctions(string text, List<string> auditTrail)
    {
        var list = new List<FunctionClassification>();

        if (ContainsAny(text, IlfKeywords))
        {
            list.Add(new FunctionClassification("ILF", "Detected internal persistence terms (e.g., banco/tabela/persistir)."));
            auditTrail.Add("Data function ILF identified from internal data keyword match.");
        }

        if (ContainsAny(text, EifKeywords))
        {
            list.Add(new FunctionClassification("EIF", "Detected external integration terms (e.g., externo/api externa/terceiro)."));
            auditTrail.Add("Data function EIF identified from external data keyword match.");
        }

        return list;
    }

    private static int ResolveDet(CountBasicRequest request, string normalizedText, List<string> auditTrail)
    {
        if (request.Det is > 0)
        {
            auditTrail.Add($"DET explicitly provided by request: {request.Det.Value}.");
            return request.Det.Value;
        }

        var det = normalizedText
            .Split(new[] { ' ', ',', '.', ';', ':', '-', '_', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries)
            .Where(word => word.Length >= 4)
            .Distinct()
            .Count();

        var resolved = Math.Max(1, det);
        auditTrail.Add($"DET inferred from unique relevant tokens: {resolved}.");
        return resolved;
    }

    private static int ResolveFtr(CountBasicRequest request, List<FunctionClassification> dataFunctions, List<string> auditTrail)
    {
        if (request.Ftr is > 0)
        {
            auditTrail.Add($"FTR explicitly provided by request: {request.Ftr.Value}.");
            return request.Ftr.Value;
        }

        var resolved = Math.Max(1, dataFunctions.Count);
        auditTrail.Add($"FTR inferred from identified data functions: {resolved}.");
        return resolved;
    }

    private static string ResolveComplexity(int det, int ftr)
    {
        if (ftr <= 1)
        {
            return det <= 15 ? "Low" : "Average";
        }

        if (ftr == 2)
        {
            if (det <= 4)
            {
                return "Low";
            }

            return det <= 15 ? "Average" : "High";
        }

        if (det <= 4)
        {
            return "Average";
        }

        return "High";
    }

    private static int ResolveFunctionPoints(string transactionType, string complexity)
    {
        return transactionType switch
        {
            "EI" => complexity switch
            {
                "Low" => 3,
                "Average" => 4,
                "High" => 6,
                _ => 0
            },
            "EO" => complexity switch
            {
                "Low" => 4,
                "Average" => 5,
                "High" => 7,
                _ => 0
            },
            "EQ" => complexity switch
            {
                "Low" => 3,
                "Average" => 4,
                "High" => 6,
                _ => 0
            },
            _ => 0
        };
    }

    private static bool ContainsAny(string text, IEnumerable<string> keywords)
    {
        return keywords.Any(text.Contains);
    }
}
