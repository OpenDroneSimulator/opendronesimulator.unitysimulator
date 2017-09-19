using UnityEngine;
using System.Collections;

/// <summary>
/// Copyright by Erik Nordeus http://www.habrador.com/tutorials/pid-controller/3-stabilize-quadcopter/
/// </summary>
public class PIDController  {

    float error_old = 0f;
    float error_sum = 0f;
    float error_sum2 = 0f;

    //PID parameters
    public float gain_P = 0f;
    public float gain_I = 0f;
    public float gain_D = 0f;

    private float error_sumMax = 20f;

   

    public float GetFactorFromPIDController(float error)
    {
        float output = CalculatePIDOutput(error);

        return output;
    }

    //Use this when experimenting with PID parameters
    public float GetFactorFromPIDController(float gain_P, float gain_I, float gain_D, float error)
    {
        this.gain_P = gain_P;
        this.gain_I = gain_I;
        this.gain_D = gain_D;

        float output = CalculatePIDOutput(error);

        return output;
    }

    //Use this when experimenting with PID parameters and the gains are stored in a Vector3
    public float GetFactorFromPIDController(Vector3 gains, float error)
    {
        this.gain_P = gains.x;
        this.gain_I = gains.y;
        this.gain_D = gains.z;

        float output = CalculatePIDOutput(error);

        return output;
    }

    private float CalculatePIDOutput(float error)
    {
        // Variable fuer Rueckgabe der Ausgangsgroesse 
        float output = 0f;

        // Berechne Ausgangsgroesse fuer P
        output += gain_P * error;

        // Berechne Ausgangsgroesse fuer I
        error_sum += Time.fixedDeltaTime * error;

        // Gib die Summe alle aufgezeichneten Abweichungen zurueck (minimal error_sum, maximal error_sumMax)
        this.error_sum = Mathf.Clamp(error_sum, -error_sumMax, error_sumMax);

        // Multipliziere Summe aller bisher aufgezeichneten Regelabweichungen mit dem Integrierbeiwert
        // und addiere ihn zur Ausgangsgroesse
        output += gain_I * error_sum;


        // Berechne Ausgangsgroesse fuer D
        // Subtrahiere die letzte Regelabweichung von der aktuellen und teile sie durch die Zeit seit dem letzten
        // physischen Update in Unity
        // Bilde den Differenzquotienten, indem die letzte Regelabweichung von der aktuellen subtrahiert wird und durch die Zeit seit dem letzten
        // physischen Update in Unity geteilt wird
        float d_dt_error = (error - error_old) / Time.fixedDeltaTime;

        // Speichere die aktuelle Regelabweichung als die zuletzt erhaltene Regelabweichung
        this.error_old = error;

        // Multipliziere den Differenzierbeiwert mit dem zuvor berechneten Differenzquotienten
        // und addiere ihn zur Ausgangsgroesse
        output += gain_D * d_dt_error;

        // Gib die berechnete Ausgangsgroesse zurueck
        return output;
    }
}
