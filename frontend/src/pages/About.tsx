import { Dumbbell, Target, Users, Award, CheckCircle2 } from "lucide-react";

const About = () => {
  return (
    <div className="container section-padding animate-fade-in">
      {/* Hero Section */}
      <section className="text-center" style={{ marginBottom: '5rem' }}>
        <div className="hero-badge" style={{ marginBottom: '1.5rem' }}>
          <Dumbbell className="text-primary" size={16} />
          <span className="text-xs font-bold text-primary uppercase tracking-widest">Our Legacy</span>
        </div>
        <h1 className="hero-title" style={{ fontSize: 'clamp(2.5rem, 8vw, 5rem)', marginBottom: '1.5rem' }}>
          About <span className="text-gradient italic">Beast Mode</span> Gym
        </h1>
        <p className="hero-description" style={{ maxWidth: '800px' }}>
          We don't just build muscles; we build character, discipline, and a community of high-performers.
        </p>
      </section>

      {/* Mission & Vision */}
      <section className="grid grid-cols-2 gap-8" style={{ marginBottom: '5rem' }}>
        <div className="glass-card" style={{ padding: '3rem', position: 'relative', overflow: 'hidden' }}>
          <div style={{ position: 'absolute', top: 0, left: 0, width: '4px', height: '100%', background: 'var(--primary)' }} />
          <Target className="text-primary" size={48} style={{ marginBottom: '1.5rem' }} />
          <h2 className="uppercase font-black tracking-tight" style={{ fontSize: '1.5rem', marginBottom: '1rem' }}>Our Mission</h2>
          <p className="text-muted font-medium leading-relaxed">
            To provide the most advanced training environment where technology meets grit. We empower our members to exceed their physical and mental limits through science-backed training and a relentless pursuit of excellence.
          </p>
        </div>
        <div className="glass-card" style={{ padding: '3rem', position: 'relative', overflow: 'hidden' }}>
          <div style={{ position: 'absolute', top: 0, left: 0, width: '4px', height: '100%', background: 'var(--accent)' }} />
          <Users className="text-accent" size={48} style={{ marginBottom: '1.5rem' }} />
          <h2 className="uppercase font-black tracking-tight" style={{ fontSize: '1.5rem', marginBottom: '1rem' }}>The Community</h2>
          <p className="text-muted font-medium leading-relaxed">
            At MyGym, you're not just another member. You're part of an elite pack. We foster a culture of mutual respect, where veterans mentor beginners and every PR is celebrated by the whole floor.
          </p>
        </div>
      </section>

      {/* Values Section */}
      <section style={{ marginBottom: '5rem' }}>
        <div className="text-center" style={{ marginBottom: '4rem' }}>
          <h2 className="uppercase font-black tracking-tight" style={{ fontSize: '2.5rem', marginBottom: '0.5rem' }}>Why Choose Us?</h2>
          <div style={{ height: '4px', width: '80px', background: 'var(--primary)', margin: '0 auto', borderRadius: '2px' }} />
        </div>

        <div className="grid grid-cols-4 gap-8">
          {[
            { label: "Elite Equipment", desc: "Top-tier Rogue & Hammer Strength gear." },
            { label: "Expert Coaches", desc: "Certified professionals with proven results." },
            { label: "24/7 Access", desc: "Train on your schedule, anytime." },
            { label: "Recovery Zone", desc: "Cryotherapy and infrared saunas included." }
          ].map((v, i) => (
            <div key={i} className="text-center">
              <div style={{ marginBottom: '1rem', color: 'var(--success)' }}>
                <CheckCircle2 size={40} style={{ margin: '0 auto' }} />
              </div>
              <h3 className="uppercase font-black tracking-tight" style={{ fontSize: '1.125rem', marginBottom: '0.5rem' }}>{v.label}</h3>
              <p className="text-dim font-medium" style={{ fontSize: '0.875rem' }}>{v.desc}</p>
            </div>
          ))}
        </div>
      </section>

      {/* Founder Quote */}
      <section>
        <div className="glass-card text-center" style={{ padding: '4rem', position: 'relative' }}>
          <div style={{ 
            position: 'absolute', 
            top: '-1.5rem', 
            left: '50%', 
            transform: 'translateX(-50%)',
            background: 'var(--bg-dark)',
            padding: '0.5rem',
            borderRadius: '1rem',
            border: '1px solid var(--glass-border)'
          }}>
            <Award className="text-primary" size={32} />
          </div>
          <blockquote className="font-bold italic leading-snug" style={{ fontSize: 'clamp(1.5rem, 4vw, 2.5rem)', color: 'var(--text-main)', marginBottom: '2rem' }}>
            "Success isn't always about greatness. It's about consistency. Consistent hard work leads to success. Greatness will come."
          </blockquote>
          <cite className="text-primary font-black uppercase tracking-widest" style={{ fontSize: '0.875rem' }}>— Dwayne Johnson (Inspired Leadership)</cite>
        </div>
      </section>
    </div>
  );
};

export default About;