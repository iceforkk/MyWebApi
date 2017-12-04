using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Community.IRepository;
using System.Net.Http;
using Community.WebApi.Models;
using System.Web.Security;
using Community.WebApi.Common;
using Community.DAL;

namespace Community.WebApi.Controllers
{
    public class BookController : BaseController
    {
        readonly IBookContentRepository _bookContentRepository;
        readonly IBookNameRepository _bookNameRepository;
        readonly IBookTitleRepository _bookTitleRepository;
        readonly IBookTypeRepository _bookTypeRepository;
        public BookController(
            IBookContentRepository bookContentRepository,
            IBookNameRepository bookNameRepository,
            IBookTitleRepository bookTitleRepository,
            IBookTypeRepository bookTypeRepository
            )
        {
            _bookContentRepository = bookContentRepository;
            _bookNameRepository = bookNameRepository;
            _bookTitleRepository = bookTitleRepository;
            _bookTypeRepository = bookTypeRepository;
        }

        /// <summary>
        /// 电子图书分类接口
        /// </summary>
        /// <returns></returns> 
        [HttpPost]
        [UserAuthorize]
        public HttpResponseMessage GetBookSort()
        {
            APIInvokeResult result = new APIInvokeResult();
            var bookTypes = _bookTypeRepository.GetALL()
               .ToList()
               .Select(s => new BookTypeInfo
               {
                   BookTypeId = s.BookTypeID,
                   BookTypeName = s.BookTypeName,
                   ParentId = -1
               })
             .ToList();

            result.Data = bookTypes;
            return result;
        }
        /// <summary>
        /// 图书列表接口
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns> 
        [HttpPost]
        public HttpResponseMessage GetBookInfoList(BookSearch model)
        {
            APIInvokeResult result = new APIInvokeResult();
            if (!string.IsNullOrWhiteSpace(model.BookTypeCode))
            {
                var category = _bookTypeRepository.GetEntity(m => m.BookTypeID == model.BookTypeId);

                if (category != null)
                {
                    model.BookTypeId = category.BookTypeID;
                }
                else
                {
                    result.Type = ERRORCODE.Failed;
                    result.Message = "图书分类不存在!";
                    return result;
                }
            }


            var date = _bookNameRepository.GetBookList(model.Keyword, model.Page, model.Rows, out int totcount, model.BookTypeId );
            var bookinfoLists = date
                .ToList()
                .Select(s => new BookInfo
                {
                    BookNameId = s.BookNameID,
                    BookName = s.Name,
                    BookType = _bookTypeRepository.GetEntity(m=>m.BookTypeID==s.BookTypeID).BookTypeName,
                    AutoName = s.AutoName,
                    Content = s.Content,
                    CreateTime = s.CreateTime.Value,
                    BookImg = s.Picture,
                    ClickCount = s.ClickCount.Value,
                    BookUrl = "",
                })
                .ToList();

            var json = new ResultInfo<BookInfo>
            {
                TotalCount = totcount,
                List = bookinfoLists
            };
            result.Data = json;
            return result;
        }
        /// <summary>
        /// 图书章节
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns> 
        [HttpPost]
        public HttpResponseMessage GetBookChapterInfoList(BookChapterSearch model)
        {
            APIInvokeResult result = new APIInvokeResult();
            //var chapterInfoLists = new List<ChapterInfo>(){
            //    new ChapterInfo{BookTitelID=16730,Title="一、坚持和发展中国特色社会主义",CreateTime=DateTime.Now,ParentID=-1,DownloadUrl="book_340_16730_.txt"},
            //    new ChapterInfo{BookTitelID=16731,Title="二、实现中华民族伟大复兴的中国梦",CreateTime=DateTime.Now,ParentID=-1,DownloadUrl="book_340_16731_.txt"}
            //};

            if (model.Rows == 0)
            {
                model.Rows = int.MaxValue;
            }
            var chapterInfoLists = _bookTitleRepository.GetBookTitleList(model.BookId, model.Page, model.Rows, out int totalCount)
                .ToList()
                .Select(s => new ChapterInfo
                {
                    BookTitelId = s.BookTitleID,
                    Title = s.Title,
                    CreateTime = s.CreateTime.Value,
                    DownloadUrl = "",
                    ParentId = -1
                })
                .ToList();
            var json = new ResultInfo<ChapterInfo>
            {
                TotalCount = totalCount,
                List = chapterInfoLists
            };
            result.Data = json;
            return result;
        }

        /// <summary>
        /// 图书章节内容
        /// </summary>
        /// <param name="bookTitleId">章节编号</param>
        /// <returns></returns> 
        [HttpPost]
        public HttpResponseMessage GetBookChapterContent(IdModel model)
        {
            APIInvokeResult result = new APIInvokeResult();
            var bookChapterId = model.Id;

            var date = _bookContentRepository.GetEntity(m => m.BookTitleID == model.Id);
            var json = new
            {
                BookTitleId = bookChapterId,
                BookContentId = date.BookContentID,
                CreateTime = date.CreateTime,
                Content = date.content
            };
            result.Data = json;
            return result;
        }
    }
}
