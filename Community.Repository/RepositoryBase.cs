using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Community.DAL;
using System.Data.Entity;

namespace Community.Repository
{
    public abstract class RepositoryBase<T>
       : IRepository.IRepository<T> where T : class, new()
    {
        #region 查询普通实现方案(基于Lambda表达式的Where查询)
        /// <summary>
        /// 获取所有Entity
        /// </summary>
        /// <param name="exp">Lambda条件的where</param>
        /// <returns>返回IEnumerable类型</returns>
        public virtual IEnumerable<T> GetEntities(Expression<Func<T, bool>> exp)
        {
            using (var db = new MobileAPIEntities())
            {
                return db.Set<T>().Where(exp).ToList();
            }
        }
        public virtual IEnumerable<T> GetALL()
        {
            using (var db = new MobileAPIEntities())
            {
                return db.Set<T>().ToList();
            }
        }
        /// <summary>
        /// 计算总个数(分页)
        /// </summary>
        /// <param name="exp">Lambda条件的where</param>
        /// <returns></returns>
        public virtual int GetEntitiesCount(Expression<Func<T, bool>> exp)
        {
            using (MobileAPIEntities db = new MobileAPIEntities())
            {
                return db.Set<T>().Where(exp).ToList().Count();
            }
        }
        /// <summary>
        /// 分页查询(Linq分页方式)
        /// </summary>
        /// <param name="pageNumber">当前页</param>
        /// <param name="pageSize">页码</param>
        /// <param name="orderName">lambda排序名称</param>
        /// <param name="sortOrder">排序(升序or降序)</param>
        /// <param name="exp">lambda查询条件where</param>
        /// <returns></returns>
        public virtual IEnumerable<T> GetEntitiesForPaging(int pageNumber, int pageSize, Expression<Func<T, string>> orderName, string sortOrder, Expression<Func<T, bool>> exp, out long totalCount)
        {
            using (MobileAPIEntities db = new MobileAPIEntities())
            {
                totalCount = FindBy(exp, db).Count();
                if (sortOrder == "asc") //升序排列
                {
                    return db.Set<T>().Where(exp).OrderBy(orderName).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                }
                else
                {
                    return db.Set<T>().Where(exp).OrderByDescending(orderName).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                }
            }
        }
        /// <summary>
        /// 根据条件查找满足条件的一个entites
        /// </summary>
        /// <param name="exp">lambda查询条件where</param>
        /// <returns></returns>
        public virtual T GetEntity(Expression<Func<T, bool>> exp)
        {
            using (MobileAPIEntities db = new MobileAPIEntities())
            {
                return db.Set<T>().Where(exp).SingleOrDefault();
            }
        }
        public virtual IQueryable<T> FindBy(Expression<Func<T, bool>> predicate, MobileAPIEntities db)
        {
            return FindAll(db).Where(predicate);
        }
        public virtual IQueryable<T> FindAll(MobileAPIEntities db)
        {
            return db.Set<T>();
        }
        #endregion 
        #region 增改删实现
        /// <summary>
        /// 插入Entity
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual bool Insert(T entity)
        {
            using (MobileAPIEntities db = new MobileAPIEntities())
            {
                var obj = db.Set<T>();
                obj.Add(entity);
                return db.SaveChanges() > 0;
            }
        }
        /// <summary>
        /// 更新Entity(注意这里使用的傻瓜式更新,可能性能略低)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual bool Update(T entity)
        {
            using (MobileAPIEntities db = new MobileAPIEntities())
            {
                var obj = db.Set<T>();
                obj.Attach(entity);
                db.Entry(entity).State = EntityState.Modified;
                return db.SaveChanges() > 0;
            }
        }
        /// <summary>
        /// 删除Entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual bool Delete(T entity)
        {
            using (MobileAPIEntities db = new MobileAPIEntities())
            {
                var obj = db.Set<T>();
                if (entity != null)
                {
                    obj.Attach(entity);

                    obj.Remove(entity);
                    return db.SaveChanges() > 0;
                }
                return false;
            }
        }
        #endregion 
    }
}
