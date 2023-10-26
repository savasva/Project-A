using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[System.Serializable]

public class Big5Personality
{
    public Big5Personality() { }

    public static Big5Personality neutral = new Big5Personality();

    /***
     * Agreeableness Traits
     ***/
    [Header("Agreeableness Traits")]
    #region Agreeableness
    /**
     * Primary trait for Agreeableness that informs the generation of all of its facets.
     * High = Agreeable
     * Low = Antagonistic
     **/
    [Range(-1, 1)]
    public float agreeableness;

    /**
     * How likely a colonist is to take something a face value.
     **/
    [Range(-1, 1)]
    public float trust;

    /**
     * How likely a colonist is to lie or conceal the truth.
     * AKA straightforwardness
     **/
    [Range(-1, 1)]
    public float morality;

    /**
     * How likely a colonist is to consider other people's emotions when making deicisons.
     * Gains happiness from helping others?
     **/
    [Range(-1, 1)]
    public float altruism;

    /**
     * How much a colonist feels bad for others.
     * Happiness affected by the happiness of those around them.
     **/
    [Range(-1, 1)]
    public float sympathy;

    /**
     * The "People Pleaser" stat.
     * How likely a colonist is to get along with others.
     * Low cooperation, more comfortable with confrontation.
     * High cooperation, just wants everyone to get along.
     **/
    [Range(-1, 1)]
    public float cooperation;

    /**
     * How highly a colonist thinks of their own achievements.
     * Low modesty colonists will be more likely to brag to other colonists.
     **/
    [Range(-1, 1)]
    public float modesty;
    #endregion

    /***
     * Extraversion Traits
     ***/
    [Header("Extraversion Traits")]
    #region extraversion

    /**
     * Primary trait for Agreeableness that informs the generation of all of its facets.
     * High = Extraverted
     * Low = Introverted
     **/
    [Range(-1, 1)]
    public float extroversion;

    /**
     * How personably a colonist is.
     **/
    [Range(-1, 1)]
    public float friendliness;

    /**
     * How much social interaction does a colonist need to feel fulfilled?
     * Could be a good multiplier for how much happiness is gained from conversation to require more to be fulfiled.
     **/
    [Range(-1, 1)]
    public float gregariousness;

    /**
     * How likely is a colonist to act upon their motivations?
     **/
    [Range(-1, 1)]
    public float assertiveness;

    /**
     * How much elbow greese is colonist willing to put in to achieve their goals?
     * Could be interpreted as laziness.
     **/
    [Range(-1, 1)]
    public float activityLevel;

    /**
     * How likely is a colonist to leave their comfort zone?
     * Could make colonists more likely/willing to do things that they don't enjoy.
     * Also factors into what kinds of environments a colonist enjoys (reading at home vs night club).
     **/
    [Range(-1, 1)]
    public float excitementSeeking;

    /**
     * Does this colonist's mood tend towards being high or low?
     * Base happiness of sorts.
     **/
    [Range(-1, 1)]
    public float cheerfulness;
    #endregion

    /***
     * Concientiousness Traits
     ***/
    [Header("Concientiousness Traits")]
    #region concientiousness
    [Range(-1, 1)]
    public float concientiousness;

    /**
     * How much a colonist believes in themselves.
     * Higher self efficacy colonists will set higher goals.
     * Less affecteed by personal insults.
     **/
    [Range(-1, 1)]
    public float selfEfficacy;

    /**
     * How neat a colonists's mind is.
     * Goal system could be designed so colonists with high orderliness will stick to routines.
     * Affects how stressed colonists get when things don't go according to plan.
     **/
    [Range(-1, 1)]
    public float orderliness;

    /**
     * How important commitments are to a colonist.
     * Affects how a colonist pursues their goals.
     **/
    [Range(-1, 1)]
    public float dutifulness;

    /**
     * How important a colonist's career is to them
     **/
    [Range(-1, 1)]
    public float achievementStriving;

    /**
     * How much a colonist procrastinates.
     * Less likely to do tasks they dislike.
     **/
    [Range(-1, 1)]
    public float selfDiscipline;

    /**
     * TODO: Describe what this stat does?
     * Affects how long a colonist takes to make a decision.
     * Affects how motivated a colonist must be to make a risky decision.
     **/
    [Range(-1, 1)]
    public float cautiousness;
    #endregion

    /**
     * Neuroticism Traits
     **/
    [Header("Neuroticism Traits")]
    #region neuroticism
    /**
     * Primary trait for Agreeableness that informs the generation of all of its facets.
     * High = Neurotic
     * Low = Stable
     ***/
    [Range(-1, 1)]
    public float neuroticism;

    /**
     * Affects how poorly a colonist reacts to bad things.
     * May dampen how a colonist feels about good things.
     * Affects how much memories and relationships affect colonists.
     **/
    [Range(-1, 1)]
    public float anxiety;

    /**
     * Affects how colonists react to their environment.
     * Interacts heavily with emotion system. Elevates arousal (intensiy of emotion).
     **/
    [Range(-1, 1)]
    public float anger;

    /**
     * How prone affected is the colonist to being affected by negative stimulus.
     * 
     * VERY VERY VERY important stat to handle correctly. Should be discussed.
     **/
    [Range(-1, 1)]
    public float depression;

    /**
     * How comfortable a colonist is around others who they don't know well.
     **/
    [Range(-1, 1)]
    public float selfConciousness;

    /**
     * How much a colonist resists temptation and considers consequences.
     * More prone to skipping work or overindulging.
     **/
    [Range(-1, 1)]
    public float immoderation;

    /**
     * How much a colonist responds to pressure.
     **/
    [Range(-1, 1)]
    public float vulnerability;
    #endregion

    /***
     * Openness Traits
     ***/
    [Header("Openness Traits")]
    #region openness
    /**
     * Primary trait for Agreeableness that informs the generation of all of its facets.
     * High = Agreeable
     * Low = Antagonistic
     ***/
    [Range(-1, 1)]
    public float openness;

    /**
     * Imagination contributes to creativity, both artistically and scientifically.
     **/
    [Range(-1, 1)]
    public float imagination;

    /**
     * How interested is colonist in the arts? Both creating and enjoyment from viewing.
     **/
    [Range(-1, 1)]
    public float artisticInterest;

    /**
     * How much a colonist expresses their emotions to each other.
     **/
    [Range(-1, 1)]
    public float emotionality;

    /**
     * How much a colonist enjoys being challenged mentally.
     **/
    [Range(-1, 1)]
    public float intellect;

    /**
     * How much a colonist enjoys risk.
     * Adventurous colonists need more excitement in their life. May prioritize their arousal.
     **/
    [Range(-1, 1)]
    public float adventurousness;

    /**
     * "Fight the Power" stat.
     * How a colonist feels about authority and social institutions.
     * Colonists with high liberalism are more likely to have goals beyond the well-being of the ship.
     **/
    [Range(-1, 1)]
    public float liberalism;
    #endregion

    public static Big5Personality operator+ (Big5Personality a, Big5Personality b)
    {
        Big5Personality aggregate = new Big5Personality();

        aggregate.agreeableness = a.agreeableness + b.agreeableness;
        aggregate.trust = a.trust + b.trust;
        aggregate.morality = a.morality + b.morality;
        aggregate.altruism = a.altruism + b.altruism;
        aggregate.sympathy = a.sympathy + b.sympathy;
        aggregate.cooperation = a.cooperation + b.cooperation;
        aggregate.modesty = a.modesty + b.modesty;

        aggregate.extroversion = a.agreeableness + b.agreeableness;
        aggregate.activityLevel = a.activityLevel + b.activityLevel;
        aggregate.assertiveness = a.assertiveness + b.assertiveness;
        aggregate.cheerfulness = a.cheerfulness + b.cheerfulness;
        aggregate.excitementSeeking = a.excitementSeeking + b.excitementSeeking;
        aggregate.friendliness = a.friendliness + b.friendliness;
        aggregate.gregariousness = a.gregariousness + b.gregariousness;

        aggregate.concientiousness = a.concientiousness + b.concientiousness;
        aggregate.achievementStriving = a.achievementStriving + b.achievementStriving;
        aggregate.cautiousness = a.cautiousness + b.cautiousness;
        aggregate.dutifulness = a.dutifulness + b.dutifulness;
        aggregate.orderliness = a.orderliness + b.orderliness;
        aggregate.selfDiscipline = a.selfDiscipline + b.selfDiscipline;
        aggregate.selfEfficacy = a.selfEfficacy + b.selfEfficacy;

        aggregate.neuroticism = a.neuroticism + b.neuroticism;
        aggregate.anger = a.anger + b.anger;
        aggregate.anxiety = a.anxiety + b.anxiety;
        aggregate.depression = a.depression + b.depression;
        aggregate.immoderation = a.immoderation + b.immoderation;
        aggregate.selfConciousness = a.selfConciousness + b.selfConciousness;
        aggregate.vulnerability = a.vulnerability + b.vulnerability;

        aggregate.openness = a.openness + b.openness;
        aggregate.adventurousness = a.adventurousness + b.adventurousness;
        aggregate.artisticInterest = a.artisticInterest + b.artisticInterest;
        aggregate.emotionality = a.emotionality + b.emotionality;
        aggregate.imagination = a.imagination + b.imagination;
        aggregate.intellect = a.intellect + b.intellect;
        aggregate.liberalism = a.liberalism + b.liberalism;

        return aggregate;
    }

    public static Big5Personality operator -(Big5Personality a, Big5Personality b)
    {
        Big5Personality aggregate = new Big5Personality();

        aggregate.agreeableness = a.agreeableness - b.agreeableness;
        aggregate.trust = a.trust - b.trust;
        aggregate.morality = a.morality - b.morality;
        aggregate.altruism = a.altruism - b.altruism;
        aggregate.sympathy = a.sympathy - b.sympathy;
        aggregate.cooperation = a.cooperation - b.cooperation;
        aggregate.modesty = a.modesty - b.modesty;

        aggregate.extroversion = a.agreeableness - b.agreeableness;
        aggregate.activityLevel = a.activityLevel - b.activityLevel;
        aggregate.assertiveness = a.assertiveness - b.assertiveness;
        aggregate.cheerfulness = a.cheerfulness - b.cheerfulness;
        aggregate.excitementSeeking = a.excitementSeeking - b.excitementSeeking;
        aggregate.friendliness = a.friendliness - b.friendliness;
        aggregate.gregariousness = a.gregariousness - b.gregariousness;

        aggregate.concientiousness = a.concientiousness - b.concientiousness;
        aggregate.achievementStriving = a.achievementStriving - b.achievementStriving;
        aggregate.cautiousness = a.cautiousness - b.cautiousness;
        aggregate.dutifulness = a.dutifulness - b.dutifulness;
        aggregate.orderliness = a.orderliness - b.orderliness;
        aggregate.selfDiscipline = a.selfDiscipline - b.selfDiscipline;
        aggregate.selfEfficacy = a.selfEfficacy - b.selfEfficacy;

        aggregate.neuroticism = a.neuroticism - b.neuroticism;
        aggregate.anger = a.anger - b.anger;
        aggregate.anxiety = a.anxiety - b.anxiety;
        aggregate.depression = a.depression - b.depression;
        aggregate.immoderation = a.immoderation - b.immoderation;
        aggregate.selfConciousness = a.selfConciousness - b.selfConciousness;
        aggregate.vulnerability = a.vulnerability - b.vulnerability;

        aggregate.openness = a.openness - b.openness;
        aggregate.adventurousness = a.adventurousness - b.adventurousness;
        aggregate.artisticInterest = a.artisticInterest - b.artisticInterest;
        aggregate.emotionality = a.emotionality - b.emotionality;
        aggregate.imagination = a.imagination - b.imagination;
        aggregate.intellect = a.intellect - b.intellect;
        aggregate.liberalism = a.liberalism - b.liberalism;

        return aggregate;
    }

    public static Big5Personality operator *(Big5Personality a, Big5Personality b)
    {
        Big5Personality aggregate = new Big5Personality();

        aggregate.agreeableness = a.agreeableness * b.agreeableness;
        aggregate.trust = a.trust * b.trust;
        aggregate.morality = a.morality * b.morality;
        aggregate.altruism = a.altruism * b.altruism;
        aggregate.sympathy = a.sympathy * b.sympathy;
        aggregate.cooperation = a.cooperation * b.cooperation;
        aggregate.modesty = a.modesty * b.modesty;

        aggregate.extroversion = a.agreeableness * b.agreeableness;
        aggregate.activityLevel = a.activityLevel * b.activityLevel;
        aggregate.assertiveness = a.assertiveness * b.assertiveness;
        aggregate.cheerfulness = a.cheerfulness * b.cheerfulness;
        aggregate.excitementSeeking = a.excitementSeeking * b.excitementSeeking;
        aggregate.friendliness = a.friendliness * b.friendliness;
        aggregate.gregariousness = a.gregariousness * b.gregariousness;

        aggregate.concientiousness = a.concientiousness * b.concientiousness;
        aggregate.achievementStriving = a.achievementStriving * b.achievementStriving;
        aggregate.cautiousness = a.cautiousness * b.cautiousness;
        aggregate.dutifulness = a.dutifulness * b.dutifulness;
        aggregate.orderliness = a.orderliness * b.orderliness;
        aggregate.selfDiscipline = a.selfDiscipline * b.selfDiscipline;
        aggregate.selfEfficacy = a.selfEfficacy * b.selfEfficacy;

        aggregate.neuroticism = a.neuroticism * b.neuroticism;
        aggregate.anger = a.anger * b.anger;
        aggregate.anxiety = a.anxiety * b.anxiety;
        aggregate.depression = a.depression + b.depression;
        aggregate.immoderation = a.immoderation * b.immoderation;
        aggregate.selfConciousness = a.selfConciousness * b.selfConciousness;
        aggregate.vulnerability = a.vulnerability * b.vulnerability;

        aggregate.openness = a.openness * b.openness;
        aggregate.adventurousness = a.adventurousness * b.adventurousness;
        aggregate.artisticInterest = a.artisticInterest * b.artisticInterest;
        aggregate.emotionality = a.emotionality * b.emotionality;
        aggregate.imagination = a.imagination * b.imagination;
        aggregate.intellect = a.intellect * b.intellect;
        aggregate.liberalism = a.liberalism * b.liberalism;

        return aggregate;
    }

    public static Big5Personality operator *(Big5Personality a, float b)
    {
        Big5Personality aggregate = new Big5Personality();

        aggregate.agreeableness = a.agreeableness * b;
        aggregate.trust = a.trust * b;
        aggregate.morality = a.morality * b;
        aggregate.altruism = a.altruism * b;
        aggregate.sympathy = a.sympathy * b;
        aggregate.cooperation = a.cooperation * b;
        aggregate.modesty = a.modesty * b;

        aggregate.extroversion = a.agreeableness * b;
        aggregate.activityLevel = a.activityLevel * b;
        aggregate.assertiveness = a.assertiveness * b;
        aggregate.cheerfulness = a.cheerfulness * b;
        aggregate.excitementSeeking = a.excitementSeeking * b;
        aggregate.friendliness = a.friendliness * b;
        aggregate.gregariousness = a.gregariousness * b;

        aggregate.concientiousness = a.concientiousness * b;
        aggregate.achievementStriving = a.achievementStriving * b;
        aggregate.cautiousness = a.cautiousness * b;
        aggregate.dutifulness = a.dutifulness * b;
        aggregate.orderliness = a.orderliness * b;
        aggregate.selfDiscipline = a.selfDiscipline * b;
        aggregate.selfEfficacy = a.selfEfficacy * b;

        aggregate.neuroticism = a.neuroticism * b;
        aggregate.anger = a.anger * b;
        aggregate.anxiety = a.anxiety * b;
        aggregate.depression = a.depression + b;
        aggregate.immoderation = a.immoderation * b;
        aggregate.selfConciousness = a.selfConciousness * b;
        aggregate.vulnerability = a.vulnerability * b;

        aggregate.openness = a.openness * b;
        aggregate.adventurousness = a.adventurousness * b;
        aggregate.artisticInterest = a.artisticInterest * b;
        aggregate.emotionality = a.emotionality * b;
        aggregate.imagination = a.imagination * b;
        aggregate.intellect = a.intellect * b;
        aggregate.liberalism = a.liberalism * b;

        return aggregate;
    }

    public float Magnitude()
    {
        return agreeableness + trust + morality + altruism + sympathy + cooperation + modesty + agreeableness + activityLevel + assertiveness
            + cheerfulness + excitementSeeking + friendliness + gregariousness + concientiousness + achievementStriving + cautiousness + dutifulness
            + orderliness + selfDiscipline + selfEfficacy + neuroticism + anger + anxiety + depression + immoderation + selfConciousness + vulnerability
            + openness + adventurousness + artisticInterest + emotionality + imagination + intellect + liberalism;
    }

    public float AbsMagnitude()
    {
        return Mathf.Abs(agreeableness) + Mathf.Abs(trust) + Mathf.Abs(morality) + Mathf.Abs(altruism) + Mathf.Abs(sympathy) + Mathf.Abs(cooperation)
            + Mathf.Abs(modesty) + Mathf.Abs(agreeableness) + Mathf.Abs(activityLevel) + Mathf.Abs(assertiveness) + Mathf.Abs(cheerfulness) + Mathf.Abs(excitementSeeking)
            + Mathf.Abs(friendliness) + Mathf.Abs(gregariousness) + Mathf.Abs(concientiousness) + Mathf.Abs(achievementStriving) + Mathf.Abs(cautiousness)
            + Mathf.Abs(dutifulness) + Mathf.Abs(orderliness) + Mathf.Abs(selfDiscipline) + Mathf.Abs(selfEfficacy) + Mathf.Abs(neuroticism) + Mathf.Abs(anger)
            + Mathf.Abs(anxiety) + Mathf.Abs(depression) + Mathf.Abs(immoderation) + Mathf.Abs(selfConciousness) + Mathf.Abs(vulnerability) + Mathf.Abs(openness)
            + Mathf.Abs(adventurousness) + Mathf.Abs(artisticInterest) + Mathf.Abs(emotionality) + Mathf.Abs(imagination) + Mathf.Abs(intellect) + Mathf.Abs(liberalism);
    }

    public static Big5Personality Random()
    {
        Big5Personality randy = new Big5Personality();

        randy.agreeableness = UnityEngine.Random.Range(-1f, 1f);
        randy.trust = UnityEngine.Random.Range(-1f, 1f);
        randy.morality = UnityEngine.Random.Range(-1f, 1f);
        randy.altruism = UnityEngine.Random.Range(-1f, 1f);
        randy.sympathy = UnityEngine.Random.Range(-1f, 1f);
        randy.cooperation = UnityEngine.Random.Range(-1f, 1f);
        randy.modesty = UnityEngine.Random.Range(-1f, 1f);

        randy.extroversion = UnityEngine.Random.Range(-1f, 1f);
        randy.activityLevel = UnityEngine.Random.Range(-1f, 1f);
        randy.assertiveness = UnityEngine.Random.Range(-1f, 1f);
        randy.cheerfulness = UnityEngine.Random.Range(-1f, 1f);
        randy.excitementSeeking = UnityEngine.Random.Range(-1f, 1f);
        randy.friendliness = UnityEngine.Random.Range(-1f, 1f);
        randy.gregariousness = UnityEngine.Random.Range(-1f, 1f);

        randy.concientiousness = UnityEngine.Random.Range(-1f, 1f);
        randy.achievementStriving = UnityEngine.Random.Range(-1f, 1f);
        randy.cautiousness = UnityEngine.Random.Range(-1f, 1f);
        randy.dutifulness = UnityEngine.Random.Range(-1f, 1f);
        randy.orderliness = UnityEngine.Random.Range(-1f, 1f);
        randy.selfDiscipline = UnityEngine.Random.Range(-1f, 1f);
        randy.selfEfficacy = UnityEngine.Random.Range(-1f, 1f);

        randy.neuroticism = UnityEngine.Random.Range(-1f, 1f);
        randy.anger = UnityEngine.Random.Range(-1f, 1f);
        randy.anxiety = UnityEngine.Random.Range(-1f, 1f);
        randy.depression = UnityEngine.Random.Range(-1f, 1f);
        randy.immoderation = UnityEngine.Random.Range(-1f, 1f);
        randy.selfConciousness = UnityEngine.Random.Range(-1f, 1f);
        randy.vulnerability = UnityEngine.Random.Range(-1f, 1f);

        randy.openness = UnityEngine.Random.Range(-1f, 1f);
        randy.adventurousness = UnityEngine.Random.Range(-1f, 1f);
        randy.artisticInterest = UnityEngine.Random.Range(-1f, 1f);
        randy.emotionality = UnityEngine.Random.Range(-1f, 1f);
        randy.imagination = UnityEngine.Random.Range(-1f, 1f);
        randy.intellect = UnityEngine.Random.Range(-1f, 1f);
        randy.liberalism = UnityEngine.Random.Range(-1f, 1f);

        return randy;
    }
}
