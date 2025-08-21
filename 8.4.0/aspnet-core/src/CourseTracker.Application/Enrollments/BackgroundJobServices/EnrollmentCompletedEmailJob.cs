using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Net.Mail;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.Extensions.Configuration;

namespace CourseTracker.Enrollments.BackgroundJobServices
{
    public class EnrollmentCompletedEmailJobArgs
    {
        public string LearnerEmail { get; set; }
        public string LearnerName { get; set; }
        public string CourseTitle { get; set; }
        public bool IsCompleted { get; set; }
        public double CompletionPercentage { get; set; }
    }
    public class EnrollmentCompletedEmailJob : BackgroundJob<EnrollmentCompletedEmailJobArgs>, ITransientDependency
    {
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _configuration;
        private readonly ILogger<EnrollmentCompletedEmailJob> _logger;

        public EnrollmentCompletedEmailJob(IEmailSender emailSender, ILogger<EnrollmentCompletedEmailJob> logger, IConfiguration configuration)
        {
            _emailSender = emailSender;
            _configuration = configuration;
            _logger = logger;
        }

        public override async void Execute(EnrollmentCompletedEmailJobArgs args)
        {
            string subject;
            string body;

            if (args.IsCompleted && args.CompletionPercentage == 100)
            {
                subject = $"Congratulations {args.LearnerName} on Successfully Completing the {args.CourseTitle} Course!";
                body = $@"<p>Dear {args.LearnerName},</p>

            <p>🎉 We are delighted to inform you that you have <b>successfully completed</b> the <b>{args.CourseTitle}</b> course. 
                This achievement reflects your hard work, dedication, and commitment to learning.</p>

            <p>Completing this course is an important milestone, and we encourage you to continue applying your knowledge 
                to grow your skills further and reach even greater heights in your learning journey.</p>

            <p>Keep up the excellent work, and we look forward to seeing your continued success!</p>

            <br/><p>Best regards,<br/>
                <b>The Course Tracker Team</b></p>";
            }
            else
            {
                subject = $"Enrollment Confirmed: {args.CourseTitle} Course";
                body = $@"<p>Dear {args.LearnerName},</p>

            <p>Welcome aboard! 🎉 You have been successfully <b>enrolled</b> in the <b>{args.CourseTitle}</b> course.</p>

            <p>This course is now available in your learning dashboard. We encourage you to start exploring the modules 
            and track your progress as you move forward.</p>

            <p>We’re excited to have you on this journey and look forward to your success.</p>

            <br/><p>Best regards,<br/>
                <b>The Course Tracker Team</b></p>";
            }

            await SendEmailAsync(args.LearnerEmail, subject, body);

            _logger.LogInformation($"📧 Email sent to {args.LearnerEmail} for course {args.CourseTitle} (Completed={args.IsCompleted}, Progress={args.CompletionPercentage}%)");
        }


        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            // Read values from appsettings.json
            var fromAddress = _configuration["Settings:Abp.Mailing.DefaultFromAddress"];
            var displayName = _configuration["Settings:Abp.Mailing.DefaultFromDisplayName"];
            var host = _configuration["Settings:Abp.Mailing.Smtp.Host"];
            var port = int.Parse(_configuration["Settings:Abp.Mailing.Smtp.Port"]);
            var userName = _configuration["Settings:Abp.Mailing.Smtp.UserName"];
            var password = _configuration["Settings:Abp.Mailing.Smtp.Password"];
            var enableSsl = bool.Parse(_configuration["Settings:Abp.Mailing.Smtp.EnableSsl"]);

            using (var client = new SmtpClient())
            {
                client.Host = host;
                client.Port = port;
                client.EnableSsl = enableSsl;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(userName, password);

                using (var emailMessage = new MailMessage())
                {
                    emailMessage.To.Add(new MailAddress(toEmail));
                    emailMessage.Subject = subject;
                    emailMessage.IsBodyHtml = true;
                    emailMessage.Body = message;
                    emailMessage.From = new MailAddress(fromAddress, displayName);

                    await client.SendMailAsync(emailMessage);
                }
            }
        }



    }


}
