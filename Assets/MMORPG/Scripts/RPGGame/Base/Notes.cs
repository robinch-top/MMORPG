/// 项目的注释说明

// ### 类的顶部注释，此类的功能与作用，与他相关的类与派生类
/// <summary>Class <c>Skills</c> 
/// 此类的整体描述...
/// 用于实体对象的技能组件基础类,其它类似的还有:</summary>
/// <see cref="Skills"/> - <see cref="PlayerSkills"/>
/// <see cref="Equipments"/> - <see cref="PlayerEquipment"/>
/// <see cref="Inventory"/> - <see cref="PlayerInventory"/>


// ### 一组重要方法的注释，也可用于一个重要方法及其参数的注释描述
/***************************************************************************/
/* 获取技能基本属性的一些方法                                                 *                        
/*=========================================================================*/
/// <summary> Skills, Equipment需要实现 ICombatBonus接口，
/// <para>提供通过组件获取技能或装备各项基本属性的方法。</para>
/// <list type="bullet">
/// <item name="GetHealthBonus">
/// <description>GetHealthBonus 从技能获得生命值提升</description>
/// </item>
/// <item name="GetManaBonus">
/// <description>GetManaBonus 从技能获得法力值提升 </description>
/// </item>
/// </list>
/// </summary>


// ### 在类代码中，对连续的一类属性或方法进行区块注释,纯为方便代码阅读,并不用于生成doc
////////////////////////////////////////////////////////////////////////////
// Defense相关的属性
// -> 通过这些属性可以从Combat组件获得角色的各种战斗相关属性


// ### 一个重要方法及其参数的注释描述
/***************************************************************************/
/* 方法名-什么相关的方法                                                     *                        
/*=========================================================================*/
/// <summary>Method <c>CanAdd</c> 检测背包能否放入指定数量的某种道具。
///  (<paramref name="item"/>,<paramref name="amount"/>)
/// <example> 使用示例：
/// <code>
///  // 背包容器中是否有空格或足够的堆叠量放入指定的数量道具
///  if (CanAdd(item, amount))
///  {
///      //...
///  }
///  return false;
/// </code>
/// </example>
/// </summary>
/// <param name="item">尝试添加到背包容器中的某种道具</param>
/// <param name="amount">尝试添加此种道具到背包容器中的数量</param>
/// <returns>返回一个 bool 值，有或者没有空间放入指定数量道具</returns>


// ### 普通的方法描述，头行上是方法名，2-n行是方法描述与参数与参数描述
// drag & drop /////////////////////////////////////////////////////////////
/// -><summary>Method <c>CanAdd</c> 检测背包能否放入指定数量的某种道具。
///  (<paramref name="item"/>,<paramref name="amount"/>) </summary>
/// <param name="item">尝试添加到背包容器中的某种道具</param>
/// <param name="amount">尝试添加此种道具到背包容器中的数量</param>
/// <returns>返回一个 bool 值，有或者没有空间放入指定数量道具</returns>


// ### 泛型类型或方法声明的注释
/// <summary>
/// 添加某类 <typeparamref name="T"/> 物品到自定义list 
/// </summary>
/// <typeparam name="T">list中的物品类型</typeparam>
