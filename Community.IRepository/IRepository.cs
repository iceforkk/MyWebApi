using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Community.IRepository
{
    public interface IRepository<T>

    {
        #region 查询普通实现方案(基于Lambda表达式的Where查询)
        /// <summary>
        /// 获取所有Entity
        /// </summary>
        /// <param name="exp">Lambda条件的where</param>
        /// <returns></returns>
        IEnumerable<T> GetEntities(Expression<Func<T, bool>> exp);


        IEnumerable<T> GetALL();
        /// <summary>
        /// 计算总个数(分页)
        /// </summary>
        /// <param name="exp">Lambda条件的where</param>
        /// <returns></returns>
        int GetEntitiesCount(Expression<Func<T, bool>> exp);

        /// <summary>
        /// 分页查询(Linq分页方式)
        /// </summary>
        /// <param name="pageNumber">当前页</param>
        /// <param name="pageSize">页码</param>
        /// <param name="orderName">lambda排序名称</param>
        /// <param name="sortOrder">排序(升序or降序)</param>
        /// <param name="exp">lambda查询条件where</param>
        /// <returns></returns>
        IEnumerable<T> GetEntitiesForPaging(int pageNumber, int pageSize, Expression<Func<T, string>> orderName, string sortOrder, Expression<Func<T, bool>> exp, out long totalCount);

        /// <summary>
        /// 根据条件查找
        /// </summary>
        /// <param name="exp">lambda查询条件where</param>
        /// <returns></returns>
        T GetEntity(Expression<Func<T, bool>> exp);



        #endregion

        //#endregion
        /// <summary>
        /// 插入Entity
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool Insert(T entity);
        /// <summary>
        /// 更新Entity
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool Update(T entity);
        /// <summary>
        /// 删除Entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool Delete(T entity);
        /// <summary>
        /// 删除实现 存储过程实现方式(调用spDelete+表名+ 主键ID)
        /// </summary>
        /// <param name="ID">删除的主键</param>
        /// <returns></returns>
        //bool Delete(object ID);
    }
}
