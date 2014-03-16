using System;
using MiniPL.Exceptions;
using MiniPL.Tokens;

namespace MiniPL.AbstractSyntaxTree
{
    /// @author Jani Viherväs
    /// @version 8.3.2014
    ///
    /// <summary>
    /// Evaluatable expression
    /// </summary>
    public class Expression
    {
        /// <summary>
        /// First operand
        /// </summary>
        public Token FirstOperand { get; private set; }

        /// <summary>
        /// Second operand
        /// </summary>
        public Token SecondOperand { get; private set; }

        /// <summary>
        /// Operator symbol
        /// </summary>
        public Token Operator { get; set; }

        /// <summary>
        /// First expression
        /// </summary>
        public Expression FirstExpression { get; private set; }

        /// <summary>
        /// Second expression
        /// </summary>
        public Expression SecondExpression { get; private set; }


        /// <summary>
        /// Adds an operand to the expression. 
        /// </summary>
        /// <param name="token">Token to add as an operand</param>
        /// <param name="addToFirst">Whether to add operand as first operand</param>
        public void AddOperand(Token token, bool addToFirst)
        {
            if ( addToFirst )
            {
                FirstOperand = token;
            }
            else
            {
                SecondOperand = token;
            }
        }

        /// <summary>
        /// Adds an expression to this
        /// </summary>
        /// <param name="expr">Expression to add</param>
        /// <param name="addToFirst">Whether to add expression as first expression</param>
        public void AddExpression(Expression expr, bool addToFirst)
        {
            if ( addToFirst )
            {
                FirstExpression = expr;
            }
            else
            {
                SecondExpression = expr;
            }
        }
        
        protected Token GetFirstNotNullToken()
        {
            if ( FirstExpression != null && FirstExpression.GetFirstNotNullToken() != null )
            {
                return FirstExpression.GetFirstNotNullToken();
            }
            if ( FirstOperand != null )
            {
                return FirstOperand;
            }
            if ( Operator != null )
            {
                return Operator;
            }
            if ( SecondOperand != null )
            {
                return SecondOperand;
            }
            if ( SecondExpression != null && SecondExpression.GetFirstNotNullToken() != null )
            {
                return SecondExpression.GetFirstNotNullToken();
            }
            return null;
        }

        /// <summary>
        /// Evaluates this expression
        /// </summary>
        /// <returns>Int result</returns>
        public int EvaluateInt()
        {
            // Six possibilities:
            // opnd
            // expr
            // opnd opnd
            // opnd expr
            // expr opnd
            // expr expr

            if ( OperandOnly() )
            {
                return ExtractInt(FirstOperand);
            }
            if ( ExpressionOnly() )
            {
                return FirstExpression.EvaluateInt();
            }
            if ( OperandOperand() )
            {
                return Calculate(GetFirstNotNullToken(), ExtractInt(FirstOperand), Operator.Lexeme, ExtractInt(SecondOperand));
            }
            if ( OperandExpression() )
            {
                return Calculate(GetFirstNotNullToken(), ExtractInt(FirstOperand), Operator.Lexeme, SecondExpression.EvaluateInt());
            }
            if ( ExpressionOperand() )
            {
                return Calculate(GetFirstNotNullToken(), FirstExpression.EvaluateInt(), Operator.Lexeme, ExtractInt(SecondOperand));
            }
            if ( ExpressionExpression() )
            {
                return Calculate(GetFirstNotNullToken(), FirstExpression.EvaluateInt(), Operator.Lexeme, SecondExpression.EvaluateInt());
            }
            Statements.AddSemanticError(GetFirstNotNullToken(), "Could not evaluate value.");
            throw new AbstractSyntaxTreeException();
        }


        /// <summary>
        /// Evaluates this expression
        /// </summary>
        /// <returns>String result</returns>
        public string EvaluateString()
        {
            // Six possibilities:
            // opnd
            // expr
            // opnd opnd
            // opnd expr
            // expr opnd
            // expr expr

            if ( OperandOnly() )
            {
                return ExtractString(FirstOperand);
            }
            if ( ExpressionOnly() )
            {
                return ExtractString(FirstExpression);
            }
            if ( OperandOperand() )
            {
                return Calculate(GetFirstNotNullToken(), ExtractString(FirstOperand), Operator.Lexeme, ExtractString(SecondOperand));
            }
            if ( OperandExpression() )
            {
                return Calculate(GetFirstNotNullToken(), ExtractString(FirstOperand), Operator.Lexeme, ExtractString(SecondExpression));
            }
            if ( ExpressionOperand() )
            {
                return Calculate(GetFirstNotNullToken(), ExtractString(FirstExpression), Operator.Lexeme, ExtractString(SecondOperand));
            }
            if ( ExpressionExpression() )
            {
                return Calculate(GetFirstNotNullToken(), ExtractString(FirstExpression), Operator.Lexeme, ExtractString(SecondExpression));
            }
            Statements.AddSemanticError(GetFirstNotNullToken(), "Could not evaluate value.");
            throw new AbstractSyntaxTreeException();
        }


        /// <summary>
        /// Evaluates this expression
        /// </summary>
        /// <returns>Boolean result</returns>
        public bool EvaluateBool()
        {
            // Six possibilities:
            // opnd
            // expr
            // opnd opnd
            // opnd expr
            // expr opnd
            // expr expr

            if ( OperandOnly() )
            {
                return ExtractBool(FirstOperand);
            }
            if ( ExpressionOnly() )
            {
                return FirstExpression.EvaluateBool();
            }
            if ( OperandOperand() )
            {
                return CalculateBool(FirstOperand, Operator.Lexeme, SecondOperand);
            }
            if ( OperandExpression() )
            {
                return CalculateBool(FirstOperand, Operator.Lexeme, SecondExpression);
            }
            if ( ExpressionOperand() )
            {
                return CalculateBool(FirstExpression, Operator.Lexeme, SecondOperand);
            }
            if ( ExpressionExpression() )
            {
                return CalculateBool(FirstExpression, Operator.Lexeme, SecondExpression);
            }
            Statements.AddSemanticError(GetFirstNotNullToken(), "Could not evaluate value.");
            throw new AbstractSyntaxTreeException();
        }


        /// <summary>
        /// Compares two integer values
        /// </summary>
        /// <param name="token"> </param>
        /// <param name="firstValue">First value</param>
        /// <param name="op">Operator</param>
        /// <param name="secondValue">Second value</param>
        /// <returns>Comparison</returns>
        private static bool Compare(Token token, int firstValue, string op, int secondValue)
        {
            switch (op)
            {
                case Operators.Equal:
                    return firstValue == secondValue;
                case Operators.NotEqual:
                    return firstValue != secondValue;
                case Operators.GreaterThan:
                    return firstValue > secondValue;
                case Operators.GreaterOrEqualThan:
                    return firstValue >= secondValue;
                case Operators.LesserThan:
                    return firstValue < secondValue;
                case Operators.LesserOrEqualThan:
                    return firstValue <= secondValue;
            }
            Statements.AddSemanticError(token, "Can't compare values int and int with operator " + op);
            throw new AbstractSyntaxTreeCalculateException("");

        }


        /// <summary>
        /// Compares two string values
        /// </summary>
        /// <param name="token"> </param>
        /// <param name="firstValue">First value</param>
        /// <param name="op">Operator</param>
        /// <param name="secondValue">Second value</param>
        /// <returns>Comparison</returns>
        private static bool Compare(Token token, string firstValue, string op, string secondValue)
        {
            switch ( op )
            {
                case Operators.Equal:
                    return firstValue == secondValue;
                case Operators.NotEqual:
                    return firstValue != secondValue;
            }
            Statements.AddSemanticError(token, "Can't compare values string and string with operator " + op);
            throw new AbstractSyntaxTreeCalculateException("");
        }


        /// <summary>
        /// Calculates boolean value from two operands
        /// </summary>
        /// <param name="first">First operand</param>
        /// <param name="op">Operator</param>
        /// <param name="second">Second operand</param>
        /// <returns>Result</returns>
        private static bool CalculateBool(Token first, string op, Token second)
        {
            try
            {
                var b1 = ExtractBool(first);
                var b2 = ExtractBool(second);
                return Calculate(first, b1, op, b2);
            }
            catch ( AbstractSyntaxTreeException )
            {
                Statements.DeleteLastAddedError();
            }
            try
            {
                var i1 = ExtractInt(first);
                var i2 = ExtractInt(second);
                return Compare(first, i1, op, i2);
            }
            catch ( AbstractSyntaxTreeException )
            {
                Statements.DeleteLastAddedError();
            }
            try
            {
                var s1 = ExtractString(first);
                var s2 = ExtractString(second);
                return Compare(first, s1, op, s2);
            }
            catch ( AbstractSyntaxTreeException )
            {
                Statements.DeleteLastAddedError();
            }
            Statements.AddSemanticError(first, "Could not evaluate value.");
            throw new AbstractSyntaxTreeCalculateException("");
        }


        /// <summary>
        /// Calculates boolean value from operand and expression
        /// </summary>
        /// <param name="first">First operand</param>
        /// <param name="op">Operator</param>
        /// <param name="second">Second expression</param>
        /// <returns>Result</returns>
        private static bool CalculateBool(Token first, string op, Expression second)
        {
            try
            {
                var b1 = ExtractBool(first);
                var b2 = second.EvaluateBool();
                return Calculate(first, b1, op, b2);
            }
            catch ( AbstractSyntaxTreeException )
            {
                Statements.DeleteLastAddedError();
            }
            try
            {
                var i1 = ExtractInt(first);
                var i2 = second.EvaluateInt();
                return Compare(first, i1, op, i2);
            }
            catch ( AbstractSyntaxTreeException )
            {
                Statements.DeleteLastAddedError();
            }
            try
            {
                var s1 = ExtractString(first);
                var s2 = ExtractString(second);
                return Compare(first, s1, op, s2);
            }
            catch ( AbstractSyntaxTreeException )
            {
                Statements.DeleteLastAddedError();
            }
            Statements.AddSemanticError(first, "Could not evaluate value.");
            throw new AbstractSyntaxTreeCalculateException("Couldn't evaluate.");
        }


        /// <summary>
        /// Calculates boolean value from expression and operand
        /// </summary>
        /// <param name="first">First expression</param>
        /// <param name="op">Operator</param>
        /// <param name="second">Second operand</param>
        /// <returns>Result</returns>
        private static bool CalculateBool(Expression first, string op, Token second)
        {
            try
            {
                var b1 = first.EvaluateBool();
                var b2 = ExtractBool(second);
                return Calculate(first.GetFirstNotNullToken(), b1, op, b2);
            }
            catch ( AbstractSyntaxTreeException )
            {
                Statements.DeleteLastAddedError();
            }
            try
            {
                var i1 = first.EvaluateInt();
                var i2 = ExtractInt(second);
                return Compare(first.GetFirstNotNullToken(), i1, op, i2);
            }
            catch ( AbstractSyntaxTreeException )
            {
                Statements.DeleteLastAddedError();
            }
            try
            {
                var s1 = first.EvaluateString();
                var s2 = ExtractString(second);
                return Compare(first.GetFirstNotNullToken(), s1, op, s2);
            }
            catch ( AbstractSyntaxTreeException )
            {
                Statements.DeleteLastAddedError();
            }
            Statements.AddSemanticError(first.GetFirstNotNullToken(), "Could not evaluate value.");
            throw new AbstractSyntaxTreeCalculateException("Couldn't evaluate.");
        }


        /// <summary>
        /// Calculates boolean value from two expressions
        /// </summary>
        /// <param name="first">First expression</param>
        /// <param name="op">Operator</param>
        /// <param name="second">Second expression</param>
        /// <returns>Result</returns>
        private static bool CalculateBool(Expression first, string op, Expression second)
        {
            try
            {
                var b1 = first.EvaluateBool();
                var b2 = second.EvaluateBool();
                return Calculate(first.GetFirstNotNullToken(), b1, op, b2);
            }
            catch ( AbstractSyntaxTreeException )
            {
                Statements.DeleteLastAddedError();
            }
            try
            {
                var i1 = first.EvaluateInt();
                var i2 = second.EvaluateInt();
                return Compare(first.GetFirstNotNullToken(), i1, op, i2);
            }
            catch ( AbstractSyntaxTreeException )
            {
                Statements.DeleteLastAddedError();
            }
            try
            {
                var s1 = first.EvaluateString();
                var s2 = second.EvaluateString();
                return Compare(first.GetFirstNotNullToken(), s1, op, s2);
            }
            catch ( AbstractSyntaxTreeException )
            {
                Statements.DeleteLastAddedError();
            }
            Statements.AddSemanticError(first.GetFirstNotNullToken(), "Could not evaluate value.");
            throw new AbstractSyntaxTreeCalculateException("Couldn't evaluate.");
        }


        /// <summary>
        /// Calculates boolean value from two operands
        /// </summary>
        /// <param name="token"> </param>
        /// <param name="firstValue">First value</param>
        /// <param name="op">Operator</param>
        /// <param name="secondValue">Second value</param>
        /// <returns>Result</returns>
        private static bool Calculate(Token token, bool firstValue, string op, bool secondValue)
        {
            switch (op)
            {
                case Operators.Equal:
                    return firstValue == secondValue;
                case Operators.NotEqual:
                    return firstValue != secondValue;
                case Operators.And:
                    return firstValue != secondValue;
            }
            Statements.AddSemanticError(token, String.Format("Can't apply operator '{0}' to operands 'bool' and 'bool'.", op));
            throw new AbstractSyntaxTreeCalculateException("");
        }


        /// <summary>
        /// Calculates integer value from two operands
        /// </summary>
        /// <param name="token"> </param>
        /// <param name="firstValue">First value</param>
        /// <param name="op">Operator</param>
        /// <param name="secondValue">Second value</param>
        /// <returns>Result</returns>
        private static int Calculate(Token token, int firstValue, string op, int secondValue)
        {
            switch ( op )
            {
                case Operators.Plus:
                    return firstValue + secondValue;
                case Operators.Minus:
                    return firstValue - secondValue;
                case Operators.Multiply:
                    return firstValue * secondValue;
                case Operators.Divide:
                    return firstValue / secondValue;
            }
            Statements.AddSemanticError(token, String.Format("Can't apply operator '{0}' to operands 'int' and int'.", op));
            throw new AbstractSyntaxTreeCalculateException("");
        }


        /// <summary>
        /// Calculates string value from two operands
        /// </summary>
        /// <param name="token"> </param>
        /// <param name="firstValue">First value</param>
        /// <param name="op">Operator</param>
        /// <param name="secondValue">Second value</param>
        /// <returns>Result</returns>
        private static string Calculate(Token token, string firstValue, string op, string secondValue)
        {
            if (op == Operators.Plus)
            {
                return firstValue + secondValue;
            }
            Statements.AddSemanticError(token, String.Format("Can't apply operator '{0}' to a string operation.", op));
            throw new AbstractSyntaxTreeCalculateException("");
        }


        /// <summary>
        /// Extracts integer value from terminal token or from variable token
        /// </summary>
        /// <param name="token">Terminal or variable token</param>
        /// <returns>Value</returns>
        private static int ExtractInt(Token token)
        {
            var tokenTerminal = token as TokenTerminal<int>;
            if ( tokenTerminal != null )
            {
                return tokenTerminal.Value;
            }
            var tokenIdentifier = token as TokenIdentifier;
            if ( tokenIdentifier != null )
            {
                var variable = Statement.GetVariable(tokenIdentifier.Identifier) as VariableType<int>;
                if ( variable != null )
                {
                    return variable.Value;
                }
            }
            Statements.AddSemanticError(token, "Was expecting an int value.");
            throw new AbstractSyntaxTreeException("");
        }


        /// <summary>
        /// Extracts string value from a terminal token or variable. 
        /// Basicly this is ToString() for tokenterminal and variables of type t
        /// </summary>
        /// <param name="token">Terminal or variable token</param>
        /// <returns>Value</returns>
        private static string ExtractString(Token token)
        {
            var tokenTerminal = token as TokenTerminal<string>;
            if ( tokenTerminal != null )
            {
                return tokenTerminal.Value;
            }
            var tokenIdentifier = token as TokenIdentifier;
            if ( tokenIdentifier != null )
            {
                var s = Statement.GetVariable(tokenIdentifier.Identifier) as VariableType<string>;
                if ( s != null )
                {
                    return s.Value;
                }
                var i = Statement.GetVariable(tokenIdentifier.Identifier) as VariableType<int>;
                if ( i != null )
                {
                    return i.Value.ToString();
                }
                var b = Statement.GetVariable(tokenIdentifier.Identifier) as VariableType<bool>;
                if ( b != null )
                {
                    return b.Value.ToString().ToLower();
                }
            }
            try
            {
                return ExtractInt(token).ToString();
            }
            catch ( AbstractSyntaxTreeException )
            {
                Statements.DeleteLastAddedError();
            }
            try
            {
                return ExtractBool(token).ToString().ToLower();
            }
            catch ( AbstractSyntaxTreeException )
            {
                Statements.DeleteLastAddedError();
            }
            Statements.AddSemanticError(token, "Was expecting a string value.");
            throw new AbstractSyntaxTreeException("");
        }


        /// <summary>
        /// Extracts string value from expression
        /// </summary>
        /// <param name="expression">Expression</param>
        /// <returns>String value</returns>
        private static string ExtractString(Expression expression)
        {
            try
            {
                return expression.EvaluateInt().ToString();
            }
            catch ( AbstractSyntaxTreeException )
            {
                Statements.DeleteLastAddedError();
            }
            try
            {
                return expression.EvaluateBool().ToString().ToLower();
            }
            catch ( AbstractSyntaxTreeException )
            {
                Statements.DeleteLastAddedError();
            }
            return expression.EvaluateString();
        }



        /// <summary>
        /// Extracts boolean value from terminal token or from variable token
        /// </summary>
        /// <param name="token">Terminal or variable token</param>
        /// <returns>Value</returns>
        private static bool ExtractBool(Token token)
        {
            var tokenTerminal = token as TokenTerminal<bool>;
            if ( tokenTerminal != null )
            {
                return tokenTerminal.Value;
            }
            var tokenIdentifier = token as TokenIdentifier;
            if ( tokenIdentifier != null )
            {
                var variable = Statement.GetVariable(tokenIdentifier.Identifier) as VariableType<bool>;
                if ( variable != null )
                {
                    return variable.Value;
                }
            }
            Statements.AddSemanticError(token, "Was expecting a boolean value.");
            throw new AbstractSyntaxTreeException("");
        }

        
        /// <summary>
        /// This expression has only the left operand
        /// </summary>
        /// <returns>Only left operand</returns>
        private bool OperandOnly()
        {
            return FirstOperand != null &&
                   SecondOperand == null && Operator == null && FirstExpression == null && SecondExpression == null;
        }


        /// <summary>
        /// This expression has only left expression
        /// </summary>
        /// <returns>Only left expression</returns>
        private bool ExpressionOnly()
        {
            return FirstExpression != null &&
                   FirstOperand == null && SecondOperand == null && Operator == null && SecondExpression == null;
        }


        /// <summary>
        /// This expression has both operands and operator, but not expressions
        /// </summary>
        /// <returns>Only two operands and operator</returns>
        private bool OperandOperand()
        {
            return FirstOperand != null && SecondOperand != null && Operator != null &&
                   FirstExpression == null && SecondExpression == null;
        }


        /// <summary>
        /// This expression has only left operand, right expression and operator
        /// </summary>
        /// <returns>Only left operand, right expression and operator</returns>
        private bool OperandExpression()
        {
            return FirstOperand != null && SecondExpression != null && Operator != null &&
                   FirstExpression == null && SecondOperand == null;
        }


        /// <summary>
        /// This expression has only left expression, right operand and operator
        /// </summary>
        /// <returns>Only left expression, right operand and operator</returns>
        private bool ExpressionOperand()
        {
            return FirstExpression != null && SecondOperand != null && Operator != null &&
                   FirstOperand == null && SecondExpression == null;
        }


        /// <summary>
        /// This expression has both expressions and operator, but not operands
        /// </summary>
        /// <returns>Only two expressions and operator</returns>
        private bool ExpressionExpression()
        {
            return FirstExpression != null && SecondExpression != null && Operator != null &&
                   FirstOperand == null && SecondOperand == null;
        }
    }
}
