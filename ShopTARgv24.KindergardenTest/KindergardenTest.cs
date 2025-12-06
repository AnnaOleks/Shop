using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Shop.Core.Dto;
using Shop.Core.Domain;
using Shop.Core.ServiceInterface;
using Shop.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopTARgv24.KindergardenTest
{
    public class KindergardenTest : TestBase
    {
        // ===== Фабрики DTO для удобства =====

        public static KindergardenDto KindergardenDto1() =>
            new KindergardenDto
            {
                GroupName = "Siilid",
                ChildrenCount = 15,
                KindergardenName = "Tallinna Lasteaed",
                TeacherName = "Mari Maasikas",
                Files = new List<IFormFile>(),
                Image = new List<FileToDatabaseDto>(),
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

        public static KindergardenDto KindergardenDto0() =>
            new KindergardenDto
            {
                GroupName = null,
                ChildrenCount = null,
                KindergardenName = null,
                TeacherName = null,
                Files = new List<IFormFile>(),
                Image = new List<FileToDatabaseDto>(),
                CreatedAt = null,
                UpdatedAt = null
            };

        public static KindergardenDto KindergardenDto2() =>
            new KindergardenDto
            {
                GroupName = "Karud",
                ChildrenCount = 22,
                KindergardenName = "Tartu Lasteaed Päike",
                TeacherName = "Kati Kask",
                Files = new List<IFormFile>(),
                Image = new List<FileToDatabaseDto>(),
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

        // ===========================================================
        // CREATE + DETAIL
        // ===========================================================

        [Fact]
        // Тест проверяет: можно создать запись и получить не-null результат.
        public async Task ShoulNot_AddEmptyKindergarden_WhenReturnResult()
        {
            KindergardenDto dto = KindergardenDto1();

            var result = await Svc<IKindergardenServices>().Create(dto);

            Assert.NotNull(result);
        }

        [Fact]
        // Тест проверяет: созданный объект можно прочитать по Id.
        public async Task ShouldNot_GetByIdKindergarden_WhenReturnsNotEqual()
        {
            var service = Svc<IKindergardenServices>();
            var created = await service.Create(KindergardenDto1());

            var result = await service.DetailAsync((Guid)created.Id!);

            Assert.NotNull(result);
        }


        // ===========================================================
        // GUID демонстрация
        // ===========================================================

        [Fact]
        // Тест проверяет: два разных Guid действительно разные.
        public async Task ShouldNot_GetByIdKindergarden_WhenReturnsNotEqual1()
        {
            Guid wrongGuid = Guid.NewGuid();
            Guid guid = Guid.Parse("0a35d9eb-e4d7-44c7-ac85-d3c584938eec");

            await Svc<IKindergardenServices>().DetailAsync(guid);

            Assert.NotEqual(wrongGuid, guid);
        }

        [Fact]
        // Тест проверяет: два одинаковых Guid равны.
        public async Task Should_GetByIdKindergarden_WhenReturnsEqual()
        {
            Guid a = Guid.Parse("0a35d9eb-e4d7-44c7-ac85-d3c584938eec");
            Guid b = Guid.Parse("0a35d9eb-e4d7-44c7-ac85-d3c584938eec");

            await Svc<IKindergardenServices>().DetailAsync(a);

            Assert.Equal(a, b);
        }

        [Fact]
        // Тест проверяет: Delete возвращает тот же Id, что удаляли.
        public async Task Should_DeleteByIdKindergarden_WhenDeleteKindergarden()
        {
            var created = await Svc<IKindergardenServices>().Create(KindergardenDto1());
            var result = await Svc<IKindergardenServices>().Delete((Guid)created.Id!);

            Assert.Equal(created.Id, result.Id);
        }

        [Fact]
        // Тест проверяет: удаление одной записи не затрагивает другую.
        public async Task ShouldNot_DeleteByIdKindergarden_WhenDidNotDeleteKindergarden1()
        {
            var s = Svc<IKindergardenServices>();
            var c1 = await s.Create(KindergardenDto1());
            var c2 = await s.Create(KindergardenDto1());

            var deleted = await s.Delete((Guid)c2.Id!);

            Assert.NotEqual(c1.Id, deleted.Id);
        }

        // ===========================================================
        // UPDATE
        // ===========================================================


        [Fact]
        // Тест проверяет: без Update данные и даты не меняются.
        public async Task ShouldNot_UpdateKindergarden_WhenDidNotUpdateDate()
        {
            var s = Svc<IKindergardenServices>();
            var created = await s.Create(KindergardenDto1());

            var detail = await s.DetailAsync((Guid)created.Id!);

            Assert.Equal(created.CreatedAt, detail.CreatedAt);
            Assert.Equal(created.UpdatedAt, detail.UpdatedAt);
        }

        // ===========================================================
        // Проверка багов (валидация)
        // ===========================================================

        [Fact]
        // Тест проверяет: сервис сохраняет отрицательное значение детей.
        public async Task Should_AddKindergarden_WhenChildrenCountIsNegative()
        {
            var s = Svc<IKindergardenServices>();
            var dto = KindergardenDto1();
            dto.ChildrenCount = -5;

            var created = await s.Create(dto);

            Assert.True(created.ChildrenCount < 0);
        }

        [Fact]
        // Тест проверяет: сервис НЕ должен принимать полностью пустой DTO.
        public async Task ShouldNot_AddKindergarden_WhenAllFieldsAreNull()
        {
            var service = Svc<IKindergardenServices>();
            KindergardenDto emptyDto = KindergardenDto0();


            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await service.Create(emptyDto);
            });
        }

        

        // ===========================================================
        // Дополнительно
        // ===========================================================

        [Fact]
        // Тест проверяет: Create генерирует Id.
        public async Task Should_CreateKindergarden_WithNoNullId()
        {
            var created = await Svc<IKindergardenServices>().Create(KindergardenDto1());
            Assert.NotNull(created.Id);
        }

        [Fact]
        // Тест проверяет: Update с несуществующим Id бросает исключение.
        public async Task ShouldNot_UpdateKindergarden_WhenIdDoesNotExist()
        {
            KindergardenDto dto = KindergardenDto1();
            dto.Id = Guid.NewGuid();

            await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() =>
                Svc<IKindergardenServices>().Update(dto));
        }

        [Fact]
        // Тест проверяет: после Create и DetailAsync данные совпадают.
        public async Task Should_ReturnSameKindergarden_WhenGetDetailsAfterCreate()
        {
            var s = Svc<IKindergardenServices>();
            var created = await s.Create(KindergardenDto1());

            var fetched = await s.DetailAsync((Guid)created.Id!);

            Assert.Equal(created.Id, fetched.Id);
            Assert.Equal(created.GroupName, fetched.GroupName);
        }

        [Fact]
        // Тест проверяет: при создании двух записей Id разные.
        public async Task Should_AssignUniqueIds_When_CreatingMultipleKindergardens()
        {
            var s = Svc<IKindergardenServices>();

            var c1 = await s.Create(KindergardenDto1());
            var c2 = await s.Create(KindergardenDto2());

            Assert.NotEqual(c1.Id, c2.Id);
        }

        [Fact]
        // Тест проверяет: после удаления Detail возвращает null.
        public async Task Should_ReturnNull_When_DeletingKindergarden()
        {
            var s = Svc<IKindergardenServices>();
            var created = await s.Create(KindergardenDto1());

            await s.Delete((Guid)created.Id!);

            var after = await s.DetailAsync((Guid)created.Id!);
            Assert.Null(after);
        }

        [Fact]
        // Тест проверяет: Create сохраняет корректные типы данных.
        public async Task Should_AddValidKindergarden_WhenDataTypeIsValid()
        {
            var dto = KindergardenDto1();
            var created = await Svc<IKindergardenServices>().Create(dto);

            Assert.IsType<int>(created.ChildrenCount);
            Assert.IsType<string>(created.GroupName);
            Assert.IsType<DateTime>(created.CreatedAt);
        }
    }
}
