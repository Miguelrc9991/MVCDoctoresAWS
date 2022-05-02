using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVCDoctoresAWS.Models;
using MVCDoctoresAWS.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MVCDoctoresAWS.Controllers
{
    public class DoctoresController : Controller
    {
        private ServiceDoctores service;
        private ServiceS3 serviceS3;
        public DoctoresController(ServiceDoctores service ,ServiceS3 serviceS3)
        {
            this.service = service;
            this.serviceS3 = serviceS3;
        }
        public async Task<IActionResult> Index()
        {
            List<Doctor> doctores =
                await this.service.GetDoctoresAsync();
            return View(doctores);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Doctor doctor, IFormFile file)
        {
            string extension = file.FileName.Split(".")[1];
            string fileName = doctor.IdDoctor.Trim() + "." + extension;
            using (Stream stream = file.OpenReadStream())
            {
                await this.serviceS3.UploadFileAsync(stream, fileName);
            }
            doctor.Imagen = fileName;
            await this.service.InsertDoctorAsync(doctor);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> UpdateDoctor(string id)
        {
            Doctor d = await this.service.GetDoctoreAsync(id);
            return View(d);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateDoctor(string IdDoctor, string IdHospital, string Apellido, string Especialidad, int Salario, IFormFile imagenBBDD)
        {
            Doctor d = await this.service.GetDoctoreAsync(IdDoctor);
            string fileName = d.Imagen;
            if (imagenBBDD != null)
            {
                string extension = imagenBBDD.FileName.Split(".")[1];
                fileName = d.IdDoctor + "." + extension;
                using (Stream stream = imagenBBDD.OpenReadStream())
                {
                    await this.serviceS3.UploadFileAsync(stream, fileName);
                }
            }
            d.IdHospital = IdHospital;
            d.Apellido = Apellido;
            d.Especialidad = Especialidad;
            d.Salario = Salario;
            d.Imagen = fileName;
            await this.service.UpdateDoctoresAsync(d);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(string id)
        {
            Doctor doc = await this.service.GetDoctoreAsync(id);
            await this.serviceS3.DeleteFileAsync(doc.Imagen);
            await this.service.DeleteDoctor(id);
            return RedirectToAction("Index");
        }

    }
}
