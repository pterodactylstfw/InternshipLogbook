using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using InternshipLogbook.API.Models;

namespace InternshipLogbook.API.Services
{
    public class WordExportService
    {
        public byte[] GenerateLogbook(Student student, List<DailyActivity> activities, string templatePath)
        {
            byte[] templateBytes = File.ReadAllBytes(templatePath);

            using (MemoryStream ms = new MemoryStream())
            {
                ms.Write(templateBytes, 0, templateBytes.Length);

                using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(ms, true))
                {
                    var body = wordDoc.MainDocumentPart.Document.Body;

                    // elimin din document erori formatare
                    body.Descendants<ProofError>().ToList().ForEach(p => p.Remove());
                    
                    NormalizeRuns(body); // pt a verifica daca placeholders sunt in acelasi elem text

                    var facultyName = student.StudyProgramme?.Faculty?.Name ?? "";
                    var studyProgName = student.StudyProgramme?.Name ?? "";
                    var companyName = student.Company?.Name ?? "";
                    var companyDesc = student.Company?.Description ?? "";

                    ReplaceText(body, "{{FirstAcademicYear}}", "2025");
                    ReplaceText(body, "{{SecondAcademicYear}}", "2026");

                    ReplaceText(body, "{{StudentName}}", student.FullName);
                    ReplaceText(body, "{{FacultyName}}", facultyName);
                    ReplaceText(body, "{{StudyProgramme}}", studyProgName);
                    ReplaceText(body, "{{Year}}", student.YearOfStudy.ToString());
                    ReplaceText(body, "{{InternshipPeriod}}", student.InternshipPeriod ?? "");

                    ReplaceText(body, "{{HostInstitution}}", companyName);
                    ReplaceText(body, "{{HostInstitutionDescription}}", companyDesc);
                    ReplaceText(body, "{{HostInternshipCoordinator}}", student.HostTutor ?? "");

                    ReplaceText(body, "{{CoordinatorName}}", student.InternshipDirector ?? "");
                    ReplaceText(body, "{{InternshipOfficer}}", "Secretariat");
                    ReplaceText(body, "{{DepartmentDirector}}", "Director Departament");

                    // tabele activitati
                    GenerateActivityTables(body, activities);

                    // pagina final
                    ReplaceText(body, "{{Eval_Quality}}", student.EvaluationQuality ?? "");
                    ReplaceText(body, "{{Eval_Comm}}", student.EvaluationCommunication ?? "");
                    ReplaceText(body, "{{Eval_Learn}}", student.EvaluationLearning ?? "");
                    ReplaceText(body, "{{Grade}}", student.SuggestedGrade?.ToString() ?? "_");

                    string currentDate = DateTime.Now.ToString("dd.MM.yyyy");
                    ReplaceText(body, "{{EvaluationDate}}", currentDate);

                    var eval = student.InternshipEvaluation;

                    // comp specifice
                    SetSkillMarker(body, eval?.TaskSequencing,      "{{S1_B}}", "{{S1_I}}", "{{S1_A}}");
                    SetSkillMarker(body, eval?.Documentation,       "{{S2_B}}", "{{S2_I}}", "{{S2_A}}");
                    SetSkillMarker(body, eval?.TheoreticalKnowledge,"{{S3_B}}", "{{S3_I}}", "{{S3_A}}");
                    SetSkillMarker(body, eval?.ToolsUsage,          "{{S4_B}}", "{{S4_I}}", "{{S4_A}}");
                    SetSkillMarker(body, eval?.Measurements,        "{{S5_B}}", "{{S5_I}}", "{{S5_A}}");
                    SetSkillMarker(body, eval?.DataRecording,       "{{S6_B}}", "{{S6_I}}", "{{S6_A}}");
                    SetSkillMarker(body, eval?.GraphicPlans,        "{{S7_B}}", "{{S7_I}}", "{{S7_A}}");
                    SetSkillMarker(body, eval?.Techniques,          "{{S8_B}}", "{{S8_I}}", "{{S8_A}}");
                    SetSkillMarker(body, eval?.SafetyFeatures,      "{{S9_B}}", "{{S9_I}}", "{{S9_A}}");
                    SetSkillMarker(body, eval?.ManualAssembly,      "{{S10_B}}", "{{S10_I}}", "{{S10_A}}");
                    SetSkillMarker(body, eval?.ElectricalAssembly,  "{{S11_B}}", "{{S11_I}}", "{{S11_A}}");
                    SetSkillMarker(body, eval?.Design,              "{{S12_B}}", "{{S12_I}}", "{{S12_A}}");
                    SetSkillMarker(body, eval?.Testing,             "{{S13_B}}", "{{S13_I}}", "{{S13_A}}");

                    // comp generale
                    SetSkillMarker(body, eval?.CompanyKnowledge,    "{{G1_B}}", "{{G1_I}}", "{{G1_A}}");
                    SetSkillMarker(body, eval?.CommunicationTeam,   "{{G2_B}}", "{{G2_I}}", "{{G2_A}}");
                    SetSkillMarker(body, eval?.CommunicationClients,"{{G3_B}}", "{{G3_I}}", "{{G3_A}}");
                    SetSkillMarker(body, eval?.Reflection,          "{{G4_B}}", "{{G4_I}}", "{{G4_A}}");
                    SetSkillMarker(body, eval?.EthicalConduct,      "{{G5_B}}", "{{G5_I}}", "{{G5_A}}");
                    SetSkillMarker(body, eval?.OrgCulture,          "{{G6_B}}", "{{G6_I}}", "{{G6_A}}");
                    SetSkillMarker(body, eval?.CareerAnalysis,      "{{G7_B}}", "{{G7_I}}", "{{G7_A}}");
                    
                    wordDoc.MainDocumentPart.Document.Save();
                }

                return ms.ToArray();
            }
        }


        private void NormalizeRuns(OpenXmlElement root) // folosit pt a verifica ca placeholderii fac parte din acelasi elem text
        {
            foreach (var paragraph in root.Descendants<Paragraph>().ToList())
            {
                var runs = paragraph.Elements<Run>().ToList();
                if (runs.Count <= 1) continue;
                
                string fullText = string.Join("", runs.Select(r => r.InnerText)); // concat prin run
                
                if (!fullText.Contains("{{")) continue; // nu normalizam daca nu are {{
                
                var firstRun = runs[0];
                var runProperties = firstRun.Elements<RunProperties>().FirstOrDefault()?.CloneNode(true);
                
                foreach (var run in runs)
                {
                    run.Remove();
                }
                
                var newRun = new Run(); // cream un nou run care sa contina tot textul concatenat
                if (runProperties != null)
                {
                    newRun.Append(runProperties);
                }

                newRun.Append(new Text(fullText) { Space = SpaceProcessingModeValues.Preserve });

                paragraph.Append(newRun);
            }
        }

        private void GenerateActivityTables(Body body, List<DailyActivity> activities) // generare tabel activitati
        {
            var masterTable = body.Descendants<Table>()
                .FirstOrDefault(t => t.InnerText.Contains("{{D_1}}")); // identif tabel principal care contine header

            if (masterTable == null) return;

            var chunks = activities.Chunk(3).ToList();
            int chunkIndex = 0;

            foreach (var chunk in chunks)
            {
                Table newTable = (Table)masterTable.CloneNode(true);
                
                NormalizeRuns(newTable);

                for (int i = 0; i < 3; i++)
                {
                    int colIndex = i + 1;
                    int realDayNumber = (chunkIndex * 3) + colIndex;
                    var act = (i < chunk.Length) ? chunk[i] : null;

                    string headerDay = (act != null) ? $"Day {act.DayNumber}" : $"Day {realDayNumber}";

                    ReplaceTextInElement(newTable, $"{{{{Day_Header_{colIndex}}}}}", headerDay);
                    ReplaceTextInElement(newTable, $"{{{{D_{colIndex}}}}}",
                        act?.DateOfActivity?.ToString("dd.MM.yyyy") ?? "");
                    ReplaceTextInElement(newTable, $"{{{{T_{colIndex}}}}}", act?.TimeFrame ?? "");
                    ReplaceTextInElement(newTable, $"{{{{V_{colIndex}}}}}", act?.Venue ?? "");
                    ReplaceTextInElement(newTable, $"{{{{Act_{colIndex}}}}}", act?.Activities ?? "");
                    ReplaceTextInElement(newTable, $"{{{{Eq_{colIndex}}}}}", act?.EquipmentUsed ?? "");
                    ReplaceTextInElement(newTable, $"{{{{Sk_{colIndex}}}}}", act?.SkillsPracticed ?? "");
                    ReplaceTextInElement(newTable, $"{{{{Obs_{colIndex}}}}}", act?.Observations ?? "");
                }

                masterTable.Parent.InsertBefore(newTable, masterTable);

                if (chunkIndex < chunks.Count - 1)
                {
                    Paragraph pageBreak = new Paragraph(new Run(new Break() { Type = BreakValues.Page }));
                    masterTable.Parent.InsertBefore(pageBreak, masterTable);
                }

                chunkIndex++;
            }

            masterTable.Remove();
        }

        private void ReplaceText(Body body, string placeholder, string value) // inlocuire placeholder - val
        {
            foreach (var text in body.Descendants<Text>())
            {
                if (text.Text.Contains(placeholder))
                    text.Text = text.Text.Replace(placeholder, value);
            }
        }

        private void ReplaceTextInElement(OpenXmlElement element, string placeholder, string value)
        {
            foreach (var text in element.Descendants<Text>())
            {
                if (text.Text.Contains(placeholder))
                    text.Text = text.Text.Replace(placeholder, value);
            }
        }
        private void SetSkillMarker(Body body, int? level, string tokenBeginner, string tokenIntermediate, string tokenAdvanced)
        {
            if (level == null) // daca nu e notat in tabel, atunci sterg toate cele 3 optiuni
            {
                ReplaceText(body, tokenBeginner, "");
                ReplaceText(body, tokenIntermediate, "");
                ReplaceText(body, tokenAdvanced, "");
                return;
            }
            
            ReplaceText(body, tokenBeginner, (level == 1) ? "X" : ""); // beginner
            
            ReplaceText(body, tokenIntermediate, (level == 2) ? "X" : ""); // intermediate
            
            ReplaceText(body, tokenAdvanced, (level == 3) ? "X" : ""); // advanced
        }
    }
    
}